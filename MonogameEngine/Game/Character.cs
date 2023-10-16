using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameEngine
{
    public partial class MonogameEngine
    {
        public class Character
        {
            public Sprite Sprite;
            public AnimationSystem AnimationSystem;

            public Direction Direction = Direction.E;
            // we can move up/down and face east/west
            // this is a death&tactics feature but going to work with this for now
            // might make this a special type of character if we have to
            // will likely just overwrite this in a game that works differently
            public Direction MovingDirection = Direction.E;

            public bool Moving = false;
            public bool Attacking = false;

            public void AnimateIdle(float speed = 1)
            {
                this.Sprite.Animations.Clear();

                this.Moving = false;
                this.Attacking = false;

                switch (this.Direction)
                {
                    case Direction.E:
                        this.AnimationSystem.Sequences["IdleRight"].PlayStatic(this.Sprite.GetScreen(), speed: speed);
                        break;
                    case Direction.W:
                        this.AnimationSystem.Sequences["IdleLeft"].PlayStatic(this.Sprite.GetScreen(), speed: speed);
                        break;
                }
            }

            public void AnimateAttack(Direction direction, Action callback = null, float speed = 1, Direction dir = Direction.E)
            {
                this.Sprite.Animations.Clear();

                this.Moving = false;
                this.Attacking = true;

                switch (this.Direction)
                {
                    case Direction.E:
                        this.Direction = direction;
                        this.AnimationSystem.Sequences["AttackRight"].PlayStatic(this.Sprite.GetScreen(), speed, callback);
                        break;
                    case Direction.W:
                        this.Direction = direction;
                        this.AnimationSystem.Sequences["AttackLeft"].PlayStatic(this.Sprite.GetScreen(), speed, callback);
                        break;
                    default: Oops(); break;
                }
            }

            public WalkAnimation AnimateWalking(Direction direction, float delta, Action callback = null, float speed = 1f, Func<Animation, bool> checkComplete = null)
            {
                this.Sprite.Animations.Clear();

                this.MovingDirection = direction;
                this.Moving = true;

                switch (direction)
                {
                    case Direction.N:

                        return (WalkAnimation)this.AnimationSystem.Sequences["WalkUp"].PlayMove(this.Sprite.GetScreen(), delta, speed, callback, checkComplete);

                    case Direction.S:

                        return (WalkAnimation)this.AnimationSystem.Sequences["WalkDown"].PlayMove(this.Sprite.GetScreen(), delta, speed, callback, checkComplete);

                    case Direction.E:

                        this.Direction = direction;

                        return (WalkAnimation)this.AnimationSystem.Sequences["WalkLeft"].PlayMove(this.Sprite.GetScreen(), delta, speed, callback, checkComplete);

                    case Direction.W:

                        this.Direction = direction;

                        return (WalkAnimation)this.AnimationSystem.Sequences["WalkLeft"].PlayMove(this.Sprite.GetScreen(), delta, speed, callback, checkComplete);
                }

                return null;
            }
        }
    }
}
