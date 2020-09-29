/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CustomBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Entry entry;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and names the object based on the result.
    /// </summary>

    private float scale = 1;
    private float x = 1;
    GameObject graph = null;
    string votesString = "0";

    // Use this for initialization
    void Start()
    {
        // Add RemoteTransformations script to object and set its entry
        this.gameObject.AddComponent<RemoteTransformations>().entry = entry;

        // Qurey additional data to get the name
        string value = "";
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("name", out value))
        {
            // Set name
            this.gameObject.name = value;
        }

        VideoPlayer videoPlayer = this.GetComponent<VideoPlayer>();
        if (videoPlayer)
            videoPlayer.isLooping = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (entry.getAdditionalData() != null) {
            // Qurey additional data to get the scale
            string scaleString = "";
            if (entry.getAdditionalData().TryGetValue("scale", out scaleString))
            {
                scale = float.Parse(scaleString);
            }
            string xString = "";
            if (entry.getAdditionalData().TryGetValue("x", out xString))
            {
                x = float.Parse(xString);
            }
            // Qurey additional data to get electoral votes
            if (entry.getAdditionalData().TryGetValue("votes", out votesString))
            {
                // Parse number of votes
                int votes = int.Parse(votesString);
                // Check for existing data
                if (graph != null){
                    // Scale graph
                    graph.transform.localScale = new Vector3(1f, votes, 1f);
                    graph.transform.position = this.gameObject.transform.position + Mathf.Sign(x) * Vector3.right * 15;
                } else {
                    // Create bar
                    GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    bar.GetComponent<Renderer>().material.color = new Color(0.75f,0,1.3f,0.1f);
                    // Create base
                    GameObject barBase = new GameObject("GraphBase " + this.gameObject.name);
                    barBase.AddComponent<MeshFilter>();
                    // Set base position
                    barBase.transform.position = new Vector3(0, -bar.transform.localScale.y, 0);
                    // Set base as parent
                    bar.transform.parent = barBase.transform;
                    // Save graph
                    graph = barBase;
                    graph.transform.parent = GameObject.Find("echoAR").transform;
                    // Scale graph
                    graph.transform.localScale = new Vector3(1f, votes, 1f);
                    // Set graph name
                    graph.name = "Graph " + this.gameObject.name;
                    // Set graph location
                    graph.transform.position = this.gameObject.transform.position + Mathf.Sign(x) * Vector3.right * 15;
                    // Add text
                    GameObject text = new GameObject();
                    TextMesh t = text.AddComponent<TextMesh>();
                    t.text = votesString;
                    t.fontSize = 200;
                    text.name = "Text " + this.gameObject.name;
                    text.transform.localScale = 0.1f * Vector3.one;
                    text.transform.position = graph.transform.position;
                    text.transform.eulerAngles = new Vector3(0, 0, 0);
                    text.transform.parent = graph.transform;
                }
            } else {
                Destroy(GameObject.Find("GraphBase " + this.gameObject.name));
            }
        }
    }
}