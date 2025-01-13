using System.Collections;
using Project.Game.Scripts;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class AudioSoundsService : MonoBehaviour
    {
        private const float CapsuleFlightDuration = 4.5f;
        private const float CapsuleExplosionDelay = 2.5f;
        private const int CountSounds = 3;
        private const bool IsAutoExpandPool = true;
    
        [SerializeField] private GunSound _gunSoundPrefab;
        [SerializeField] private MiningStoneSound _miningStoneSoundPrefab;
        [SerializeField] private MachineGunSound _machineGunSoundPrefab;
        [SerializeField] private MinesSound _minesSoundPrefab;
        [SerializeField] private GrenadesSound _grenadesSoundPrefab;
        [SerializeField] private CapsuleFlightSound _capsuleFlightSoundPrefab;
        [SerializeField] private CapsuleExplosionSound _capsuleExplosionSoundPrefab;
        [SerializeField] private CardViewButtonSound _cardViewButtonSoundPrefab;
        [SerializeField] private FourBarrelMachineGunSound _fourBarrelMachineGunSoundPrefab;
        [SerializeField] private ButtonSound _buttonSoundPrefab;

        private ObjectPool<GunSound> _poolGunSoundsOfShots;
        private ObjectPool<MiningStoneSound> _poolMiningSoundsOfStone;
        private ObjectPool<MachineGunSound> _poolMachineGunSounds;
        private ObjectPool<FourBarrelMachineGunSound> _poolFourBarrelMachineGunSounds;

        private MinesSound _minesSound;
        private GrenadesSound _grenadesSound;
        private CapsuleFlightSound _capsuleFlightSound;
        private CapsuleExplosionSound _capsuleExplosionSound;
        private CardViewButtonSound _cardViewButtonSound;
        private ButtonSound _buttonSound;

        private void Awake()
        {
            _poolGunSoundsOfShots = new ObjectPool<GunSound>(_gunSoundPrefab, CountSounds, transform);
            _poolMiningSoundsOfStone = new ObjectPool<MiningStoneSound>(_miningStoneSoundPrefab, CountSounds, transform);
            _poolMachineGunSounds = new ObjectPool<MachineGunSound>(_machineGunSoundPrefab, CountSounds, transform);
            _poolFourBarrelMachineGunSounds =
                new ObjectPool<FourBarrelMachineGunSound>(_fourBarrelMachineGunSoundPrefab, CountSounds, transform);

            _minesSound = Instantiate(_minesSoundPrefab, transform);
            _grenadesSound = Instantiate(_grenadesSoundPrefab, transform);
            _capsuleFlightSound = Instantiate(_capsuleFlightSoundPrefab, transform);
            _capsuleExplosionSound = Instantiate(_capsuleExplosionSoundPrefab, transform);
            _cardViewButtonSound = Instantiate(_cardViewButtonSoundPrefab, transform);
            _buttonSound = Instantiate(_buttonSoundPrefab, transform);

            _poolGunSoundsOfShots.AutoExpand = IsAutoExpandPool;
            _poolMiningSoundsOfStone.AutoExpand = IsAutoExpandPool;
            _poolMachineGunSounds.AutoExpand = IsAutoExpandPool;
            _poolFourBarrelMachineGunSounds.AutoExpand = IsAutoExpandPool;
        }

        public void PlaySound(Sounds sound)
        {
            switch (sound)
            {
                case Sounds.Gun :
                    PlayGunSound();
                    break;
                case Sounds.Stone :
                    PlaySoundOfMiningStone();
                    break;
                case Sounds.MachineGun :
                    PlayMachineGunSound();
                    break;
                case Sounds.Mines :
                    PlayMinesSound();
                    break;
                case Sounds.FragGrenades :
                    PlayGrenadesSound();
                    break;
                case Sounds.CapsuleFlight :
                    PlayCapsuleFlightSound();
                    break;
                case Sounds.CardViewButton :
                    PlayCardViewButtonSound();
                    break;
                case Sounds.FourBarrelMachineGun :
                    PlayFourBarrelMachineGunSound();
                    break;
                case Sounds.Button :
                    PlayButtonSound();
                    break;
            }
        }

        private void PlayGunSound()
        {
            GunSound sound = _poolGunSoundsOfShots.GetFreeElement();
        
            sound.AudioSource.PlayOneShot(sound.AudioSource.clip);

            StartCoroutine(sound.OffPoolSoundAfterPlay());
        }

        private void PlaySoundOfMiningStone()
        {
            MiningStoneSound miningStoneSound = _poolMiningSoundsOfStone.GetFreeElement();
        
            miningStoneSound.AudioSource.PlayOneShot(miningStoneSound.AudioSource.clip);

            StartCoroutine(miningStoneSound.OffPoolSoundAfterPlay());
        }

        private void PlayMachineGunSound()
        {
            MachineGunSound machineGunSound = _poolMachineGunSounds.GetFreeElement();
        
            machineGunSound.AudioSource.PlayOneShot(machineGunSound.AudioSource.clip);

            StartCoroutine(machineGunSound.OffPoolSoundAfterPlay());
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
            StartCoroutine(_capsuleFlightSound.OffSoundAfterDurationPlay(CapsuleFlightDuration));
        }
    
        private void PlayCardViewButtonSound()
        {
            _cardViewButtonSound.AudioSource.PlayOneShot(_cardViewButtonSoundPrefab.AudioSource.clip);
        }

        private void PlayFourBarrelMachineGunSound()
        {
            FourBarrelMachineGunSound fourBarrelMachineGunSound = _poolFourBarrelMachineGunSounds.GetFreeElement();
        
            fourBarrelMachineGunSound.AudioSource.PlayOneShot(fourBarrelMachineGunSound.AudioSource.clip);

            StartCoroutine(fourBarrelMachineGunSound.OffPoolSoundAfterPlay());
        }

        private void PlayButtonSound()
        {
            _buttonSound.AudioSource.PlayOneShot(_buttonSound.AudioSource.clip);
        }
    
        private IEnumerator PlayCapsuleExplosionSound()
        {
            yield return new WaitForSeconds(CapsuleExplosionDelay);
        
            _capsuleExplosionSound.AudioSource.PlayOneShot(_capsuleExplosionSound.AudioSource.clip);
        }
    }
}
