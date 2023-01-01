using UnityEngine;
using Zenject;

public class TestController : MonoBehaviour
{
    [Inject] private Test Test { get; set; }
}