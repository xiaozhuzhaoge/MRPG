// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Audio)]
    [Tooltip("Plays an Audio Clip at a position defined by a Game Object or Vector3. If a position is defined, it takes priority over the game object. This action doesn't require an Audio Source component, but offers less control than Audio actions.")]
    public class FM_PlaySoundByPercend : FsmStateAction
    {
        public FsmOwnerDefault gameObject;

        public FsmVector3 position;

        [RequiredField]
        [Title("Audio Clip")]
        [ObjectType(typeof(AudioClip))]
        public FsmObject clip;

        [HasFloatSlider(0, 1)]
        public FsmFloat volume = 1f;
        public FsmFloat percent = 1f;
        Animator ani;
        public override void Reset()
        {
            gameObject = null;
            position = new FsmVector3 { UseVariable = true };
            clip = null;
            volume = 1;
        }
        bool isPLAY;
        public override void OnEnter()
        {
            isPLAY = false;
            ani = Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<Animator>();
         
        }

        public override void OnUpdate()
        {
            if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > percent.Value)
            {
                if (!isPLAY)
                    DoPlaySound();
            }
        }

        void DoPlaySound()
        {
            var audioClip = clip.Value as AudioClip;

            if (audioClip == null)
            {
                LogWarning("Missing Audio Clip!");
                return;
            }
             
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume.Value);
            isPLAY = true;
        }

#if UNITY_EDITOR
        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, clip);
        }
#endif

    }
}