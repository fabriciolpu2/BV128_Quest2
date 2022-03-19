using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Photon.Pun;

namespace RootMotion.Demos
{

public class VRIK_PUN_G : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
{

    #region All

    [Tooltip("Root of the VR camera rig")] public GameObject vrRig;
    [Tooltip("The VRIK component.")] public VRIK ik;

    // NetworkTransforms are network snapshots of Transform position, rotation, velocity and angular velocity
    private NetworkTransform rootNetworkT = new NetworkTransform();
    private NetworkTransform headNetworkT = new NetworkTransform();
    private NetworkTransform leftHandNetworkT = new NetworkTransform();
    private NetworkTransform rightHandNetworkT = new NetworkTransform();

    // Called by Photon when the player is instantiated
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // Initiation
        if (photonView.IsMine)
        {
            //  InitiateLocal();
        }
        else
        {
            InitiateRemote();
        }

        name = "VRIK_PUN_Player " + (photonView.IsMine ? "(Local)" : "(Remote)");
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            //UpdateLocal();
        }
        else
        {
            UpdateRemote();
        }
    }

    // Sync NetworkTransforms
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send NetworkTransform data
            rootNetworkT.Send(stream);
            headNetworkT.Send(stream);
            leftHandNetworkT.Send(stream);
            rightHandNetworkT.Send(stream);
        }
        else
        {
            // Receive NetworkTransform data
            rootNetworkT.Receive(stream);
            headNetworkT.Receive(stream);
            leftHandNetworkT.Receive(stream);
            rightHandNetworkT.Receive(stream);
        }
    }

    #endregion All

    // Code that runs only for remote instances of this player
    #region Remote

    [LargeHeader("Remote")]
    [Tooltip("The speed of interpolating remote IK targets.")] public float proxyInterpolationSpeed = 20f;
    [Tooltip("Max interpolation error square magnitude. IK targets snap to latest synced position if current interpolated position is farther than that.")] public float proxyMaxErrorSqrMag = 4f;
    [Tooltip("If assigned, remote instances of this player will use this material.")] public Material remoteMaterialOverride;

    private Transform headIKProxy;
    private Transform leftHandIKProxy;
    private Transform rightHandIKProxy;

    private void InitiateRemote()
    {
        // Remote instance does not have a VR rig, so we use proxies for them. Positions and rotations of proxies are synced via NetworkTransforms
        //vrRig.SetActive(false);

        // Ceate IK target proxies
        var proxyRoot = new GameObject("IK Proxies").transform;
        proxyRoot.parent = transform;
        proxyRoot.localPosition = Vector3.zero;
        proxyRoot.localRotation = Quaternion.identity;

        headIKProxy = new GameObject("Head IK Proxy").transform;
        headIKProxy.position = ik.references.head.position;
        headIKProxy.rotation = ik.references.head.rotation;

        leftHandIKProxy = new GameObject("Left Hand IK Proxy").transform;
        leftHandIKProxy.position = ik.references.leftHand.position;
        leftHandIKProxy.rotation = ik.references.leftHand.rotation;

        rightHandIKProxy = new GameObject("Right Hand IK Proxy").transform;
        rightHandIKProxy.position = ik.references.rightHand.position;
        rightHandIKProxy.rotation = ik.references.rightHand.rotation;

        // Assign proxies as IK targets for the remote instance
        ik.solver.spine.headTarget = headIKProxy;
        ik.solver.leftArm.target = leftHandIKProxy;
        ik.solver.rightArm.target = rightHandIKProxy;

        // Just for debugging
        if (remoteMaterialOverride != null)
        {
            ik.references.root.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = remoteMaterialOverride;
        }
    }

    private void UpdateRemote()
    {
        // Apply synced position/rotations to proxies
        if (ik.solver.locomotion.weight <= 0) rootNetworkT.ApplyRemoteInterpolated(ik.references.root, proxyInterpolationSpeed, proxyMaxErrorSqrMag); // Only sync root when using animated locomotion. Procedural locomotion follows head IK proxy anyway
        headNetworkT.ApplyRemoteInterpolated(headIKProxy, proxyInterpolationSpeed, proxyMaxErrorSqrMag);
        leftHandNetworkT.ApplyRemoteInterpolated(leftHandIKProxy, proxyInterpolationSpeed, proxyMaxErrorSqrMag);
        rightHandNetworkT.ApplyRemoteInterpolated(rightHandIKProxy, proxyInterpolationSpeed, proxyMaxErrorSqrMag);
    }

    #endregion Remote
    }
}
