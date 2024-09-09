using System.Collections;
using System.Collections.Generic;
using Project.Game.Scripts;
using UnityEngine;

public class AudioSoundsService : MonoBehaviour
{
    private const string Gun = nameof(Gun);
    private const string Stone = nameof(Stone);
    private const string MachineGun = nameof(MachineGun);
    private const string Mines = nameof(Mines);
    private const string Grenades = nameof(Grenades);
    private const string CapsuleFlight = nameof(CapsuleFlight);

    private const float CapsuleFlightDuration = 4.5f;
    private const float CapsuleExplosionDelay = 2.5f;
    private const int CountSounds = 3;
    private const bool IsAutoExpandPool = true;
    
    [SerializeField] private GunSoundOfShot _gunSoundOfShotsPrefab;
    [SerializeField] private MiningStoneSound _miningStoneSoundPrefab;
    [SerializeField] private MachineGunSound _machineGunSoundPrefab;
    [SerializeField] private MinesSound _minesSoundPrefab;
    [SerializeField] private GrenadesSound _grenadesSoundPrefab;
    [SerializeField] private CapsuleFlightSound _capsuleFlightSoundPrefab;
    [SerializeField] private CapsuleExplosionSound _capsuleExplosionSoundPrefab;

    private ObjectPool<GunSoundOfShot> _poolGunSoundsOfShots;
    private ObjectPool<MiningStoneSound> _poolMiningSoundsOfStone;
    private ObjectPool<MachineGunSound> _poolMachineGunSounds;

    private MinesSound _minesSound;
    private GrenadesSound _grenadesSound;
    private CapsuleFlightSound _capsuleFlightSound;
    private CapsuleExplosionSound _capsuleExplosionSound;

    private void Awake()
    {
        _poolGunSoundsOfShots = new ObjectPool<GunSoundOfShot>(_gunSoundOfShotsPrefab, CountSounds, transform);
        _poolMiningSoundsOfStone = new ObjectPool<MiningStoneSound>(_miningStoneSoundPrefab, CountSounds, transform);
        _poolMachineGunSounds = new ObjectPool<MachineGunSound>(_machineGunSoundPrefab, CountSounds, transform);

        _minesSound = Instantiate(_minesSoundPrefab);
        _grenadesSound = Instantiate(_grenadesSoundPrefab);
        _capsuleFlightSound = Instantiate(_capsuleFlightSoundPrefab);
        _capsuleExplosionSound = Instantiate(_capsuleExplosionSoundPrefab);

        _poolGunSoundsOfShots.AutoExpand = IsAutoExpandPool;
        _poolMiningSoundsOfStone.AutoExpand = IsAutoExpandPool;
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case Gun :
                PlayGunSound();
                break;
            case Stone :
                PlaySoundOfMiningStone();
                break;
            case MachineGun :
                PlayMachineGunSound();
                break;
            case Mines :
                PlayMinesSound();
                break;
            case Grenades :
                PlayGrenadesSound();
                break;
            case CapsuleFlight :
                PlayCapsuleFlightSound();
                break;
        }
    }

    private void PlayGunSound()
    {
        GunSoundOfShot soundOfShot = _poolGunSoundsOfShots.GetFreeElement();
        
        soundOfShot.AudioSource.PlayOneShot(soundOfShot.AudioSource.clip);

        StartCoroutine(soundOfShot.OffSoundAfterPlay());
    }

    private void PlaySoundOfMiningStone()
    {
        MiningStoneSound miningStoneSound = _poolMiningSoundsOfStone.GetFreeElement();
        
        miningStoneSound.AudioSource.PlayOneShot(miningStoneSound.AudioSource.clip);

        StartCoroutine(miningStoneSound.OffSoundAfterPlay());
    }

    private void PlayMachineGunSound()
    {
        MachineGunSound machineGunSound = _poolMachineGunSounds.GetFreeElement();
        
        machineGunSound.AudioSource.PlayOneShot(machineGunSound.AudioSource.clip);

        StartCoroutine(machineGunSound.OffSoundAfterPlay());
    }
    
    private void PlayMinesSound()
    {
        _minesSound.AudioSource.PlayOneShot(_minesSound.AudioSource.clip);
    }
    
    private void PlayGrenadesSound()
    {
        _grenadesSound.AudioSource.PlayOneShot(_grenadesSound.AudioSource.clip);
    }

    private void PlayCapsuleFlightSound()
    {
        _capsuleFlightSound.AudioSource.PlayOneShot(_capsuleFlightSound.AudioSource.clip);

        StartCoroutine(PlayCapsuleExplosionSound());
        StartCoroutine(_capsuleFlightSound.OffSoundAfterPlay(CapsuleFlightDuration));
    }
    
    private IEnumerator PlayCapsuleExplosionSound()
    {
        yield return new WaitForSeconds(CapsuleExplosionDelay);
        
        _capsuleExplosionSound.AudioSource.PlayOneShot(_capsuleExplosionSound.AudioSource.clip);
    }
}
