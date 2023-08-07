using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Script.Lab
{
    public class MixChemical : MonoBehaviour
    {
        [SerializeField] private Transform tube;
        [SerializeField] private Transform flask1;
        [SerializeField] private Transform flask2;
        [SerializeField] private GameObject smellEffect;
        [SerializeField] private GameObject ewwClip;
        [SerializeField] private Animator animator;
        [SerializeField] private Material flask1Material;
        [SerializeField] private Material flask2Material;
        [SerializeField] private Material tubeMaterial;

        private int decreaseLiquidIndex;

        private void Awake()
        {
            tubeMaterial.SetFloat("_FillAmount", .5f);
            flask1Material.SetFloat("_FillAmount", .29f);
            flask2Material.SetFloat("_FillAmount", .29f);
            flask1Material.SetColor("_TopColor", Color.white);
            flask1Material.SetColor("_FoamColor", Color.white);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider != null)
                        MoveTube(hit.collider.gameObject.name);
                }
            }
        }

        private void MoveTube(string objName)
        {
            if (objName == tube.name)
                tube.DOLocalMoveY(.07f, .5f);
            else if (objName == flask1.name && tube.localPosition.y >= .068f)
            {
                DOTween.Sequence()
                    .Append(flask1.DOLocalMoveY(.6f, .5f))
                    .Append(tube.DOLocalMoveZ(-.045f, 1f))
                    .Append(tube.DORotate(new Vector3(-70, 0, 0f), 1f))
                    .Append(tube.DORotate(new Vector3(0, 0, 0), 1f))
                    .OnComplete(()=>StartCoroutine(ChangeColor()));
            }
            else if (objName == flask2.name && tube.localPosition.y >= .068f)
            {
                DOTween.Sequence()
                    .Append(flask2.DOLocalMoveY(.6f, .5f))
                    .Append(tube.DOLocalMoveZ(-.1f, 1f))
                    .Append(tube.DORotate(new Vector3(-70, 0, 0f), 1f))
                    .Append(tube.DORotate(new Vector3(0, 0, 0f), 1f))
                    .OnComplete(() => StartCoroutine(UnpleasantSmell()));
            }
        }
    
        private IEnumerator ChangeColor()
        {
            DecreaseLiquid();
            flask1Material.SetFloat("_FillAmount", .22f);
            flask1.DOShakeRotation(1.5f, 10f, 10)
                .OnComplete(()=>
                {
                    flask1Material.DOColor(Color.red, "_TopColor", .5f);
                    flask1Material.DOColor(Color.red, "_FoamColor", .5f);
                    animator.Play("ExcitedAnim");
                });
            yield return new WaitForSeconds(2f);
            animator.Play("Idle");
        }

        private IEnumerator UnpleasantSmell()
        {
            DecreaseLiquid();
            flask2Material.SetFloat("_FillAmount", .22f);
            flask2.DOShakeRotation(1.5f, 10f, 10)
                .OnComplete(()=>
                {
                    smellEffect.SetActive(true);
                    ewwClip.SetActive(true);
                    animator.Play("IrritatedAnim");
                });
            yield return new WaitForSeconds(1f);
            ewwClip.SetActive(false);
            animator.Play("Idle");
        }

        private void DecreaseLiquid()
        {
            if (decreaseLiquidIndex == 0)
            {
                decreaseLiquidIndex++;
                tubeMaterial.SetFloat("_FillAmount", .62f);
            }
            else
                tubeMaterial.SetFloat("_FillAmount", 1f);
        }
    }
}
