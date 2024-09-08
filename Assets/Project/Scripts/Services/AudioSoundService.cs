using Project.Game.Scripts;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    private const string ObjectPoolSoundsOfMiningStoneName = "PoolMiningSoundsOfStone";
    private const string ObjectPoolSoundsOfShotsName = "PoolAssaultRifleSoundsOfShots";
    
    [SerializeField] private AssaultRifleSoundOfShot _soundOfShotsPrefab;
    [SerializeField] private MiningStoneSound _miningStoneSoundPrefab;
    
    [SerializeField] private bool _isAutoExpandPool = false;

    private int _countSounds = 5;
    
    private ObjectPool<MiningStoneSound> _poolMiningSoundsOfStone;
    private ObjectPool<AssaultRifleSoundOfShot> _poolSoundsOfShots;

    private void Awake()
    {
        _poolSoundsOfShots = new ObjectPool<AssaultRifleSoundOfShot>(_soundOfShotsPrefab, _countSounds,
            new GameObject(ObjectPoolSoundsOfShotsName).transform);
        
        _poolMiningSoundsOfStone = new ObjectPool<MiningStoneSound>(_miningStoneSoundPrefab, _countSounds, 
            new GameObject(ObjectPoolSoundsOfMiningStoneName).transform);
        
        _poolSoundsOfShots.AutoExpand = _isAutoExpandPool;
        _poolMiningSoundsOfStone.AutoExpand = _isAutoExpandPool;
    }

    public void PlaySound(Sound sound)
    {
        
    }

    public void PlaySoundOfShots()
    {
        AssaultRifleSoundOfShot soundOfShot = _poolSoundsOfShots.GetFreeElement();
        
        soundOfShot.AudioSource.PlayOneShot(soundOfShot.AudioSource.clip);

        StartCoroutine(soundOfShot.OffSound());
    }

    public void PlaySoundOfMiningStone()
    {
        MiningStoneSound miningStoneSound = _poolMiningSoundsOfStone.GetFreeElement();
        
        miningStoneSound.AudioSource.PlayOneShot(miningStoneSound.AudioSource.clip);

        StartCoroutine(miningStoneSound.OffSound());
    }
}
