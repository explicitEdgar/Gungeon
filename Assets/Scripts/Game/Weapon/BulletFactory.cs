using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
    public partial class BulletFactory : ViewController
    {
        public static BulletFactory Default;
        private ResLoader mResLoader = ResLoader.Allocate();

        // 使用 SimpleObjectPool 管理各种子弹壳的对象池
        private Dictionary<string, SimpleObjectPool<GameObject>> mShellPools =
            new Dictionary<string, SimpleObjectPool<GameObject>>();

        private void Awake()
        {
            Default = this;
        }

        void Start()
        {
            // 初始化代码可以放在这里
        }

        private void OnDestroy()
        {
            Default = null;
            mResLoader.Recycle2Cache();
            mResLoader = null;

            foreach (var poolPair in mShellPools)
            {
                //摧毁对象池
                poolPair.Value.Clear();
            }
            mShellPools.Clear();
        }

        /// <summary>
        /// 生成子弹壳
        /// </summary>
        /// <param name="direction">发射方向</param>
        /// <param name="bulletShell">子弹壳预制体(可选，默认使用手枪弹壳)</param>
        public static void GenBulletShell(Vector2 direction, Rigidbody2D bulletShell = null)
        {
            if (bulletShell == null)
            {
                // 默认使用手枪弹壳
                bulletShell = GetShellFromPool("PistolShell");
            }
            else
            {
                // 检查是否有对应类型的对象池
                var shellType = GetShellType(bulletShell);
                if (!string.IsNullOrEmpty(shellType))
                {
                    bulletShell = GetShellFromPool(shellType);
                }
                else
                {
                    // 未知类型，直接实例化
                    bulletShell = bulletShell.Instantiate();
                }
            }

            if (bulletShell == null)
            {
                Debug.LogError("获取子弹壳失败");
                return;
            }

            // 设置子弹壳初始位置和物理效果
            bulletShell.transform
                .Position2D(Player.Default.Position2D() + direction * 0.5f)
                .Show()
                .Self(self =>
                {
                    var rb = self.GetComponent<Rigidbody2D>();
                    var spriteRenderer = self.GetComponent<SpriteRenderer>();

                    // 初始速度和角速度
                    var velocity = -direction * Random.Range(2, 5f) + Vector2.up * Random.Range(3, 6f);
                    rb.velocity = velocity;
                    rb.angularVelocity = Random.Range(-720, 720);
                    spriteRenderer.sortingLayerName = "Fx";

                    // 使用 ActionKit 实现子弹壳物理效果序列
                    ActionKit.Sequence()
                    .Delay(Random.Range(0.5f, 1), () =>
                    {
                        // 回弹效果
                        rb.velocity = -direction * Random.Range(0.5f, 2f) + Vector2.up * Random.Range(0, 0.5f);
                        rb.gravityScale = 0.1f;
                        rb.angularVelocity = RandomUtility.Choose(-1, 1) * Random.Range(180, 720);
                    })
                    .Parallel(p =>
                    {
                        // 播放落地音效
                        p.PlaySound($"Resources://BulletShell/bullet_shell ({Random.Range(1, 72 + 1)})")
                        // 回弹后停止
                        .Delay(Random.Range(0.1f, 0.3f), () =>
                        {
                            rb.angularVelocity = 0;
                            rb.gravityScale = 0;
                            rb.velocity = Vector2.zero;
                            spriteRenderer.sortingLayerName = "OnFloor";
                        });
                    })
                    .Delay(Random.Range(2f, 4), () =>
                    {
                        // 最终回收
                        rb.gravityScale = 0;
                        rb.angularVelocity = 0;
                        rb.velocity = Vector2.zero;
                        RecycleShell(rb);
                    })
                    .Start(Default);
                });
        }

        /// <summary>
        /// 从对象池获取子弹壳
        /// </summary>
        /// <param name="shellType">子弹壳类型</param>
        /// <returns>子弹壳 Rigidbody2D 组件</returns>
        private static Rigidbody2D GetShellFromPool(string shellType)
        {
            // 懒加载对象池
            if (!Default.mShellPools.ContainsKey(shellType))
            {
                GameObject prefab = null;
                switch (shellType)
                {
                    case "PistolShell":
                        prefab = Default.PistolShell.gameObject;
                        break;
                    case "AKShell":
                        prefab = Default.AKShell.gameObject;
                        break;
                    case "AWPShell":
                        prefab = Default.AWPShell.gameObject;
                        break;
                    case "ShotGunShell":
                        prefab = Default.ShotGunShell.gameObject;
                        break;
                }

                // 使用 SimpleObjectPool 创建对象池
                Default.mShellPools[shellType] = new SimpleObjectPool<GameObject>
                 (
                     () => GameObject.Instantiate(prefab),
                     shell => shell.SetActive(false),
                     5  // 初始数量
                 );
            }
            return Default.mShellPools[shellType].Allocate().GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// 回收子弹壳到对象池
        /// </summary>
        /// <param name="shell">要回收的子弹壳GameObject</param>
        private static void RecycleShell(Rigidbody2D shell)
        {
            var shellType = GetShellType(shell);
            if (!string.IsNullOrEmpty(shellType))
            {   
                // 有对应类型的对象池，回收子弹壳
                if(Default.mShellPools.ContainsKey(shellType) && Default.mShellPools[shellType] != null)
                {
                    Default.mShellPools[shellType].Recycle(shell.gameObject);
                    return;
                }
            }

            Debug.LogError("没有找到对应的对象池回收");
            shell.DestroySelf();
        }

        /// <summary>
        /// 根据预制体获取子弹壳类型名称
        /// </summary>
        private static string GetShellType(Rigidbody2D shellPrefab)
        {
            if (shellPrefab == null) return null;

            // 通过预制体名称比较
            var prefabName = shellPrefab.gameObject.name.Replace("(Clone)", "").Trim();
            if (prefabName == Default.PistolShell.gameObject.name) return "PistolShell";
            if (prefabName == Default.AKShell.gameObject.name) return "AKShell";
            if (prefabName == Default.AWPShell.gameObject.name) return "AWPShell";
            if (prefabName == Default.ShotGunShell.gameObject.name) return "ShotGunShell";
            return null;
        }
    }
}
