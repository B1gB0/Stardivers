using System.Collections;
using Project.Scripts.Audio.Sounds;
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
        [SerializeField] private ChainLightningGunSound _chainLightningGunSoundPrefab;

        private ObjectPool<GunSound> _poolGunSoundsOfShots;
        private ObjectPool<MiningStoneSound> _poolMiningSoundsOfStone;
        private ObjectPool<MachineGunSound> _poolMachineGunSounds;
        private ObjectPool<FourBarrelMachineGunSound> _poolFourBarrelMachineGunSounds;
        private ObjectPool<ChainLightningGunSound> _poolChainLightningGunSounds;

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
            _poolChainLightningGunSounds =
                new ObjectPool<ChainLightningGunSound>(_chainLightningGunSoundPrefab, CountSounds, transform);

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
            _poolChainLightningGunSounds.AutoExpand = IsAutoExpandPool;
        }

        public void PlaySound(SoundsType soundType)
        {
            switch (soundType)
            {
                case SoundsType.Gun :
                    PlayGunSound();
                    break;
                case SoundsType.Stone :
                    PlaySoundOfMiningStone();
                    break;
                case SoundsType.MachineGun :
                    PlayMachineGunSound();
                    break;
                case SoundsType.Mines :
                    PlayMinesSound();
                    break;
                case SoundsType.FragGrenades :
                    PlayGrenadesSound();
                    break;
                case SoundsType.ChainLightningGun :
                    PlayChainLightningGunSound();
                    break;
                case SoundsType.CapsuleFlight :
                    PlayCapsuleFlightSound();
                    break;
                case SoundsType.CardViewButton :
                    PlayCardViewButtonSound();
                    break;
                case SoundsType.FourBarrelMachineGun :
                    PlayFourBarrelMachineGunSound();
                    break;
                case SoundsType.Button :
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

        private void PlayChainLightningGunSound()
        {
            ChainLightningGunSound chainLightningGunSound = _poolChainLightningGunSounds.GetFreeElement();
            
            chainLightningGunSound.AudioSource.PlayOneShot(chainLightningGunSound.AudioSource.clip);
            
            StartCoroutine(chainLightningGunSound.OffPoolSoundAfterPlay());
        }
    
        private IEnumerator PlayCapsuleExplosionSound()
        {
            yield return new WaitForSeconds(CapsuleExplosionDelay);
        
            _capsuleExplosionSound.AudioSource.PlayOneShot(_capsuleExplosionSound.AudioSource.clip);
        }
    }
}
