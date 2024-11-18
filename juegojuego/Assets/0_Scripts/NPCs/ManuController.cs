using PixelCrushers.DialogueSystem;
using UnityEngine;

public class ManuController : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        Lua.RegisterFunction(nameof(PlayIdleManu), this, SymbolExtensions.GetMethodInfo(() => PlayIdleManu()));
        Lua.RegisterFunction(nameof(PlayEnfadaoManu), this, SymbolExtensions.GetMethodInfo(() => PlayEnfadaoManu()));
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void PlayEnfadaoManu()
    {
        animator.Play("Enfadao");
    }

    private void PlayIdleManu()
    {
        animator.Play("Idle");
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction(nameof(PlayIdleManu));
        Lua.UnregisterFunction(nameof(PlayEnfadaoManu));
    }
}
