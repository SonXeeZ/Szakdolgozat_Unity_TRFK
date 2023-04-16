using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

/*[SelectionBase]
    public class Entity : NetworkBehaviour, IDamageable, IEntityAnimation
    {
        [Header("Entity data")]
        [SerializeField] private NetworkVariable<string> Name = new NetworkVariable<string>();

        [SerializeField] private NetworkVariable<uint> ID = new NetworkVariable<uint>(0);
        //public NetworkVariable<uint> ID { get => ID ; set => ID = value; }

        //[SyncVar] public uint ID = 0;                                           // unique entity ID
        //[SyncVar] public string Name;                                           // entity name

        [Header("NavMesh")]
        public NavMeshAgent agent;                                              // entity NavMesh agent

        [Header("Attributes")]
        [SerializeField] private NetworkVariable<int> health = new NetworkVariable<int>(100);

        //[SyncVar] private int health = 100;                                     // entity current health
        public int Health                                                       // entity current health
        {
            get
            {
                return health.Value;
            }
            set
            {
                value = value > maxHealth.Value ? maxHealth.Value : value;
                if (health.Value < value)
                {
                    EventOnHeal(value - health.Value);
                }

                health.Value = value;
            }
        }
        [SerializeField] private NetworkVariable<int> maxHealth = new NetworkVariable<int>();

        //[HideInInspector] [SyncVar] public int maxHealth;                       // entity max health

        public delegate void OnDamageDelegate(uint attackerID, int value);       // event called when entity gets damaged
        public delegate void OnHealDelegate(int value);                         // event called when entity receives heal
        [SyncEvent] public event OnDamageDelegate EventOnDamage;
        [SyncEvent] public event OnHealDelegate EventOnHeal;

        [HideInInspector] public Animator m_Animator;                           // entity animator

        public bool isRunning { get; protected set; }                           // bools for synchronizing animations

        #region //======            ATTRIBUTE METHODS           ======\\

        /// <summary>
        /// Damage entity
        /// </summary>
        /// <param name="attackerID">ID of attacker</param>
        /// <param name="damage">damage</param>
        [ServerRpc]
        public virtual void Damage(uint attackerID, int damage)
        {
            Health -= damage;
            EventOnDamage(attackerID, damage);
        }

        #endregion

        #region //======            ANIMATION           ======\\

        /// <summary>
        /// Play animation if entity has Animator
        /// </summary>
        /// <param name="name">Animator state name</param>
        public virtual void PlayAnimation(string name)
        {
            if (!m_Animator) return;

            m_Animator.Play(name);
        }

        /// <summary>
        /// Set Animator trigger if entity has Animator
        /// </summary>
        /// <param name="name">Trigger variable name</param>
        public virtual void SetTrigger(string name)
        {
            if (!m_Animator) return;

            m_Animator.SetTrigger(name);
        }

        #endregion

        #region //======            EVENT LISTENERS           ======\\

        public virtual void OnHeal(int value)
        {
            Debug.Log("ON HEAL +" + value);
        }

        public virtual void OnDamage(uint attackerID, int value)
        {
            Debug.Log("ON DAMAGE -" + value);
        }

        #endregion

        #region //======            MONOBEHAVIOURS           ======\\

        private void Awake()
        {
            maxHealth.Value = Health;
            EventOnDamage += OnDamage;
            EventOnHeal += OnHeal;
        }
        public virtual void Start()
        {
        if (!IsServer)
        {
        // Remove the code related to weapons
        ObjectDatabase.AddEntity(this);
        }
        m_Animator = GetComponentInChildren<Animator>();
        }

        public virtual void LateUpdate()
        {
        if (m_Animator != null)
        {
        // check if entity is moving
        isRunning = agent.velocity != Vector3.zero;
        // set speed variable based on movement speed
            m_Animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(agent.velocity.x), Mathf.Abs(agent.velocity.z)));
        }
        }

        public virtual void OnDestroy()
        {
        // Remove the code related to weapons
        ObjectDatabase.RemoveEntity(this);
        }

        public virtual void OnEnable()
        {
        // wait for sync var to load
        StartCoroutine(WaitForID());
        }

        /// <summary>
        /// Block movement possibility
        /// </summary>
        /// <param name="time">block duration</param>
        public void BlockMovement(float time)
        {
        StartCoroutine(IEBlockMovement(time));
        }

        #region //====== COROUTINES ======\

        private IEnumerator WaitForID()
        {
        yield return null;
        ObjectDatabase.Instance.GetEntityPosition(ID);
        }

        private IEnumerator IEBlockMovement(float time)
        {
        agent.isStopped = true;
        yield return new WaitForSeconds(time);
        agent.isStopped = false;
        }

        #endregion
}*/