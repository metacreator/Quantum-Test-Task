using Quantum;
using UnityEngine;

public class AnimationView : QuantumEntityViewComponent<GameContext>
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    [SerializeField] private Animator animator;

    public override void OnUpdateView()
    {
        var kcc = PredictedFrame.Get<CharacterController3D>(EntityRef);

        animator.SetFloat(Speed, kcc.Velocity.Magnitude.AsFloat);
    }
}