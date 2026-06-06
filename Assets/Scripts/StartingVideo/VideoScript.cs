using UnityEngine;
using UnityEngine.Video;
using System.Runtime.InteropServices;

[RequireComponent(typeof(VideoPlayer))]
public class BackgroundVideo : MonoBehaviour
{
    VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Application.streamingAssetsPath + "/BackgroundM.mp4";
        videoPlayer.isLooping = true;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.Prepare();
    }

    void OnPrepareCompleted(VideoPlayer vp)
    {
        vp.Play();
    }
}