using UnityEngine;

public class WheelControl : MonoBehaviour
{
   public GameObject rightHand;
   private Transform rightHandOriginalParent;
   private bool rightHandOnWheel = false;
   
   public GameObject leftHand;
   private Transform leftHandOriginalParent;
   private bool leftHandOnWheel = false;

   public Transform[] snapPositions;

   private int numberOfHandsOnWheel;

   public GameObject vehicle;
   private Rigidbody vehiclerRB;

   public float currentWheelRotation;

   private float turnDampening;

}
