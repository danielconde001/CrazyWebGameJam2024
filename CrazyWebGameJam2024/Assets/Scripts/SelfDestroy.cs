using UnityEngine;
using System.Threading.Tasks;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private int timerInMilliseconds = 3000;

    public async void Destruct()
    {
        await Task.Delay(timerInMilliseconds);
        Destroy(gameObject);
    }
}
