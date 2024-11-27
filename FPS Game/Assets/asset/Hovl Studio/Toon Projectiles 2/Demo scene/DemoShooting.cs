using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;
using Photon.Pun;

public class DemoShooting : MonoBehaviourPun {
    public GameObject FirePoint;
    public Camera Cam;
    public float MaxLength;
    public GameObject[] Prefabs;

    private Ray RayMouse;
    private Vector3 direction;
    private Quaternion rotation;

    [Header("GUI")]
    private float windowDpi;
    private int Prefab;
    private GameObject Instance;
    private float hSliderValue = 0.1f;
    private float fireCountdown = 0f;

    //Double-click protection
    private float buttonSaver = 0f;
    private AudioSource Source;
    public AudioClip Clip;

    //For Camera shake 
    public Animation camAnim;
    PhotonView pv;
    private void Awake() {
        pv = GetComponent<PhotonView>();
        Source = GetComponent<AudioSource>();
    }
    void Start() {
        if (Screen.dpi < 1) windowDpi = 1;
        if (Screen.dpi < 200) windowDpi = 1;
        else windowDpi = Screen.dpi / 200f;
        Counter(0);
    }

    void Update() {
        if (!pv.IsMine) return;

        //Single shoot
        if (Input.GetMouseButtonDown(0)) {
            Fire();
            pv.RPC("Fire", RpcTarget.Others);
            Source.clip = Clip;
            Source.volume = 0.75f;
            Source.Play();
        }


    }




    // To change prefabs (count - prefab number)
    void Counter(int count) {
        Prefab += count;
        if (Prefab > Prefabs.Length - 1) {
            Prefab = 0;
        } else if (Prefab < 0) {
            Prefab = Prefabs.Length - 1;
        }
    }

    //To rotate fire point
    void RotateToMouseDirection(GameObject obj, Vector3 destination) {
        direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
    [PunRPC]
    public void Fire() {
        //camAnim.Play(camAnim.clip.name);
        Instantiate(Prefabs[Prefab], FirePoint.transform.position, FirePoint.transform.rotation);

    }

}
