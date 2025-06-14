using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQ.FileBase.Missile_Launcher.Missile;
using System;

/*
 *@author GameDevHQ 
 * For support, visit gamedevhq.com
 */

namespace GameDevHQ.FileBase.Missile_Launcher
{
    public class Missile_Launcher : MonoBehaviour
    {
        public enum MissileType
        {
            Normal,
            Homing
        }


        [SerializeField]
        private GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private MissileType _missileType; //type of missle to be launched
        [SerializeField]
        private GameObject[] _misslePositions; //array to hold the rocket positions on the turret
        [SerializeField]
        private float _fireDelay; //fire delay between rockets
        [SerializeField]
        private float _launchSpeed; //initial launch speed of the rocket
        [SerializeField]
        private float _power; //power to apply to the force of the rocket
        [SerializeField]
        private float _fuseDelay; //fuse delay before the rocket launches
        [SerializeField]
        private float _reloadTime; //time in between reloading the rockets
        [SerializeField]
        private float _destroyTime = 10.0f; //how long till the rockets get cleaned up
        private bool _launched; //bool to check if we launched the rockets

        private int _missileIndex; // Which missile should we be firing next?

        private int _missilesAvailable;    // How many missiles are currently available

        private float _timeForNextFire = 0;

        private void Start()
        {
            _missileIndex = 0;
            _missilesAvailable = _misslePositions.Length;
        }

        public void FireMissile(Transform target)
        {
            if (_missilesAvailable > 0 && Time.time >= _timeForNextFire)
            {
                FireRocketsRoutine(target);
                Debug.Log($"Fire delay by {_fireDelay}");
                _timeForNextFire = Time.time + _fireDelay;
                Debug.Log($"This should mean that the next time to fire is {_timeForNextFire}");
            }
        }

        void FireRocketsRoutine(Transform target)
        {
            GameObject rocket = ObjectPoolManager.Instance.GetFromPool(_missilePrefab); //instantiate a rocket

            rocket.transform.parent = _misslePositions[_missileIndex].transform; //set the rockets parent to the missile launch position 
            rocket.transform.localPosition = Vector3.zero; //set the rocket position values to zero
            rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
            rocket.transform.parent = null; //set the rocket parent to null

            rocket.GetComponent<Missile.Missile>().AssignMissleRules(_missileType, target, _launchSpeed, _power, _fuseDelay, _destroyTime); //assign missile properties 

            _misslePositions[_missileIndex].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

            StartCoroutine(ReloadMissileAfterDelay(_missileIndex));

            _missileIndex = (_missileIndex + 1) % _misslePositions.Length;
            _missilesAvailable--;
        }

        private IEnumerator ReloadMissileAfterDelay(int missileIndex)
        {
            yield return new WaitForSeconds(_reloadTime);

            _misslePositions[missileIndex].SetActive(true);
            _missilesAvailable++;
        }
    }
}

