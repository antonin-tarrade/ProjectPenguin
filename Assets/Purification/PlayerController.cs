using System.Collections;
using UnityEngine;

namespace Assets.Purification
{
    /// <summary>
    /// State machine that handles the input for the player and do the corresponding actions
    /// </summary>
    public class PlayerController: MonoBehaviour
    {

        private IPlayerState playerState;

        private void Awake()
        {
            playerState = new PlayerMovingState(gameObject);
        }

        private void Update()
        {
            playerState = playerState.HandleInput();
        }
    }


    public interface IPlayerState
    {
        public string GetName();
        public IPlayerState HandleInput();
    }

    public abstract class PlayerState : IPlayerState
    {
        public string name { get; protected set; }

        protected GameObject player;

        public string GetName()
        {
            return name;
        }

        public abstract IPlayerState HandleInput();


        public PlayerState(GameObject player)
        {
            this.player = player;
        }
    }

    public class PlayerMovingState : PlayerState
    {
        private Vector2 direction;
        private Vector2 movement;
        private Penguin penguin;
        private IAttack attack;

        public PlayerMovingState(GameObject player) : base(player)
        {
            name = "Moving";
            movement = new();
            penguin = player.GetComponent<Penguin>();
            attack = player.GetComponent<IAttack>();
        }

        public override IPlayerState HandleInput()
        {
            if (Input.GetKeyDown("space"))
            {
                penguin.StartSlide();
                return new PlayerSlidingState(player);
            }
            else
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
                if (movement.sqrMagnitude > 0) direction = movement;
                penguin.Move(movement);
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.R))
                {
                    attack.Fire(direction);
                }
                return this;
            }
        }
    }

    public class PlayerSlidingState : PlayerState
    {
        private Penguin penguin;
        public PlayerSlidingState(GameObject player) : base(player)
        {
            name = "Sliding";
            penguin = player.GetComponent<Penguin>();
        }

        public override IPlayerState HandleInput()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                penguin.StopSlide();
                return new PlayerMovingState(player);
            }
            else return this;
        }
    }
}

