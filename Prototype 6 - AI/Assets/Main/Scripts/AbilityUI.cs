using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour{
    public CharacterControllerAttacks attacks;

    public float transitionDuration = 0.2f;

    public Image centerImage;
    public Image leftImage;
    public Image rightImage;
    public Image veryLeftImage;
    public Image veryRightImage;


    private Vector3 centerPos;
    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 veryLeftPos;
    private Vector3 veryRightPos;

    private Quaternion centerRot;
    private Quaternion leftRot;
    private Quaternion rightRot;
    private Quaternion veryLeftRot;
    private Quaternion veryRightRot;

    private HashSet<Transform> currentlyMoving = new HashSet<Transform>();

    void Start(){
        centerPos = centerImage.transform.position;
        leftPos = leftImage.transform.position;
        rightPos = rightImage.transform.position;
        veryLeftPos = veryLeftImage.transform.position;
        veryRightPos = veryRightImage.transform.position;

        centerRot = centerImage.transform.rotation;
        leftRot = leftImage.transform.rotation;
        rightRot = rightImage.transform.rotation;
        veryLeftRot = veryLeftImage.transform.rotation;
        veryRightRot = veryRightImage.transform.rotation;

        PopulateImages();
    }

    public void PopulateImages(){
        centerImage.sprite = attacks.GetSelectedAttack().attackIcon;
        leftImage.sprite = attacks.GetAdjacentAttack(false).attackIcon;
        rightImage.sprite = attacks.GetAdjacentAttack(true).attackIcon;
    }

    public void ResetPositions(){
        centerImage.transform.position = centerPos;
        leftImage.transform.position = leftPos;
        rightImage.transform.position = rightPos;
        veryLeftImage.transform.position = veryLeftPos;
        veryRightImage.transform.position = veryRightPos;

        centerImage.transform.rotation = centerRot;
        leftImage.transform.rotation = leftRot;
        rightImage.transform.rotation = rightRot;
        veryLeftImage.transform.rotation = veryLeftRot;
        veryRightImage.transform.rotation = veryRightRot;

        PopulateImages();
    }

    public void Scroll(bool isDirRight, Sprite newSprite){
        if(!isDirRight){
            veryRightImage.sprite = newSprite;
            StartCoroutine(MoveAToB(veryRightImage.transform, transitionDuration, veryRightPos, rightPos, veryRightRot, rightRot));
            StartCoroutine(MoveAToB(rightImage.transform, transitionDuration, rightPos, centerPos, rightRot, centerRot));
            StartCoroutine(MoveAToB(centerImage.transform, transitionDuration, centerPos, leftPos, centerRot, leftRot));
            StartCoroutine(MoveAToB(leftImage.transform, transitionDuration, leftPos, veryLeftPos, leftRot, veryLeftRot));
            StartCoroutine(ResetTransitionWhenNotMovingAny());
        }else{
            veryLeftImage.sprite = newSprite;
            StartCoroutine(MoveAToB(veryLeftImage.transform, transitionDuration, veryLeftPos, leftPos, veryLeftRot, leftRot));
            StartCoroutine(MoveAToB(leftImage.transform, transitionDuration, leftPos, centerPos, leftRot, centerRot));
            StartCoroutine(MoveAToB(centerImage.transform, transitionDuration, centerPos, rightPos, centerRot, rightRot));
            StartCoroutine(MoveAToB(rightImage.transform, transitionDuration, rightPos, veryRightPos, rightRot, veryRightRot));
            StartCoroutine(ResetTransitionWhenNotMovingAny());
        }
    }

    private IEnumerator MoveAToB(Transform obj, float duration, Vector3 originalPos, Vector3 destPos, Quaternion originalRot, Quaternion destRot){
        while(currentlyMoving.Contains(obj)){
            yield return null;
        }

        currentlyMoving.Add(obj);

        float remainingTime = duration;
        while(remainingTime > 0f){
            obj.position = Vector3.Lerp(originalPos, destPos, 1f - remainingTime / duration);
            obj.rotation = Quaternion.Slerp(originalRot, destRot, 1f - remainingTime / duration);
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        currentlyMoving.Remove(obj);
    }

    private IEnumerator ResetTransitionWhenNotMovingAny(){
        while(currentlyMoving.Count > 0){
            yield return null;
        }

        ResetPositions();
        PopulateImages();
    }
}
