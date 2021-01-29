using System.Collections.Generic;
using UnityEngine;

public class EnemyDirectionGraphic : MonoBehaviour
{
    [SerializeField] Transform _currentDirection = null;
    [SerializeField] Transform _upDirection = null;
    [SerializeField] Transform _upRightDIrection = null;
    [SerializeField] Transform _rightDirection = null;
    [SerializeField] Transform _downRightDirection = null;
    [SerializeField] Transform _downDirection = null;
    [SerializeField] Transform _downLeftDirection = null;
    [SerializeField] Transform _leftDirection = null;
    [SerializeField] Transform _upLeftDirection = null;

    Dictionary<Direction, Transform> _elements = null;

    public void SetUp() {
        _elements = new Dictionary<Direction, Transform>();
        _elements.Add(Direction.Up, _upDirection);
        _elements.Add(Direction.UpRight, _upRightDIrection);
        _elements.Add(Direction.Right, _rightDirection);
        _elements.Add(Direction.DownRight, _downRightDirection);
        _elements.Add(Direction.Down, _downDirection);
        _elements.Add(Direction.DownLeft, _downLeftDirection);
        _elements.Add(Direction.Left, _leftDirection);
        _elements.Add(Direction.UpLeft, _upLeftDirection);
    }

    public void UpdateGraphic(Direction currentDirection, List<Direction> directionsToShow) {
        foreach (Direction directionToChange in System.Enum.GetValues(typeof(Direction))) {
            ChangeDirectionVisibility(directionToChange, /*directionToChange == currentDirection || */directionsToShow.Contains(directionToChange));
        }
        _currentDirection.rotation = currentDirection.GetAngle();
    }

    private void ChangeDirectionVisibility(Direction directionToChange, bool newVisibility) {
        if (_elements.ContainsKey(directionToChange)) {
            Transform element = _elements[directionToChange];
            if (element != null) {
                element.gameObject.SetActive(newVisibility);
            }
        }
    }
}
