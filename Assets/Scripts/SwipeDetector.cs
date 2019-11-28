using System;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPosition;                     //When you touch
    private Vector2 fingerUpPosition;                       //When you release

    [SerializeField] private bool detectSwipeOnlyAfterRelease = false;
    [SerializeField] private float minDistanceForSwipe = 20.0f;

    public static event Action<SwipeData> OnSwipe = delegate { };       //Static because this is something that's gonna be reused across the whole project.

    private void Update()
    {
        foreach ( Touch touch in Input.touches )
        {
            if ( touch.phase == TouchPhase.Began )      //If this is the first touch we assign the position or save the position of that first touch as a reference.
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if ( !detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved )  //We checked ir realeae the touch or swipe. If that's not true and Moved is true. If it is we set the finger down position to the current one.
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if ( touch.phase == TouchPhase.Ended )
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if ( SwipeDistanceCheckMet() )      //We verify if we mmoved far enough (20 pixels)
        {
            if ( IsVerticalSwipe() )
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe( direction );
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe( direction );
            }

            fingerUpPosition = fingerDownPosition;
        }
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs( fingerDownPosition.y - fingerUpPosition.y );
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs( fingerDownPosition.x - fingerUpPosition.x );
    }

    private void SendSwipe( SwipeDirection direction )          //When this method called we pass the current value.
    {
        SwipeData swipeData = new SwipeData() {         //Struct values
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        OnSwipe( swipeData );                           //We called al methos that need make something with the current touch Values.
    }

}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up, Down, Left, Right
}