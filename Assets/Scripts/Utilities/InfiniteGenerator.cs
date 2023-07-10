using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfiniteGenerator : MonoBehaviour
{
    // Ref
    public Transform ground;
    public Transform water;

    public BoxCollider StartCol;
    public BoxCollider EndCol;
    public BoxCollider LeftCol;
    public BoxCollider RightCol;

    public Transform Player;

    public Transform EndLevel;

    //Prefab
    public Object TreePrefab;
    public Object CollectorBuyPrefab;
    public Object BuffBuyPrefab;

    // Parameter
    public int levelLength = 50;
    public int levelWidth = 10;



    //Tree Spawn
    public AnimationCurve forestLengthOverLenght;
    // Bewteen 0 & 1, it will scale it with levelLenght
    public AnimationCurve TreeLifeOverLenght;
    // Bewteen 0 & 1, it will scale it with levelLenght
    public AnimationCurve TreeRessourceOverLenght;


    //SpawnedElement -- Keeped for Destroy when generate

    List<GameObject> spawnedElement = new List<GameObject>();

    // Collector spawn
    public AnimationCurve CollectorCostMinOverLength;
    public AnimationCurve CollectorCostMaxOverLength;
    public AnimationCurve ChanceForCollectorSpawnBetweenForestOverLenght;


    // Buff spawn
    public AnimationCurve BuffCostMinOverLength;
    public AnimationCurve BuffCostMaxOverLength;
    public AnimationCurve ChanceForBuffSpawnBetweenForestOverLenght;

    private void Start()
    {
        Generate();
    }

    [ContextMenu("Generate")]
    [Button("Generate")]
    public void Generate()
    {
        foreach (GameObject item in spawnedElement)
        {
            DestroyImmediate(item);

        }

        spawnedElement.Clear();

        levelLength =(int) Random.Range(40, 150);


        ground.position = Vector3.zero;
        ground.localScale = new Vector3(levelWidth, 2, levelLength);

        water.position = Vector3.zero + (Vector3.down*0.2f);
        water.localScale = new Vector3(levelWidth*4, 2, levelLength*1.5f);

        StartCol.transform.position = Vector3.zero + (Vector3.back * ((levelLength / 2) + .5f)) + (Vector3.up*2);
        StartCol.transform.localScale = new Vector3(levelWidth, 5, 2);
        EndCol.transform.position = Vector3.zero + (Vector3.forward * ((levelLength / 2) + .5f)) + (Vector3.up * 2);
        EndCol.transform.localScale = new Vector3(levelWidth, 5, 2);

        EndLevel.transform.position = Vector3.zero + (Vector3.forward * (levelLength / 2)) + (Vector3.back*5) + (Vector3.up * 1);

        LeftCol.transform.position = Vector3.zero + (Vector3.left * ((levelWidth / 2)+.5f) ) + (Vector3.up * 2);
        LeftCol.transform.localScale = new Vector3(2, 5, levelLength);
        RightCol.transform.position = Vector3.zero + (Vector3.right * ((levelWidth / 2)+ .5f)) + (Vector3.up * 2);
        RightCol.transform.localScale = new Vector3(2, 5, levelLength);

        int startPosZ = -(levelLength / 2);
        int endPosZ = (levelLength / 2);
        int currentPosCurrentZ = startPosZ;


        currentPosCurrentZ += 2;
        Player.transform.position = new Vector3(0, 2, currentPosCurrentZ);

        currentPosCurrentZ += 5;

        int nbRecoltBuy = 0;
        int nbBuffBuy = 0;

        while (currentPosCurrentZ < endPosZ - 15)
        {
            float lengthRatio = Mathf.InverseLerp(startPosZ, endPosZ, currentPosCurrentZ);
            float CollectorBuyChance = ChanceForCollectorSpawnBetweenForestOverLenght.Evaluate(lengthRatio);
            float BuffBuyChance = ChanceForBuffSpawnBetweenForestOverLenght.Evaluate(lengthRatio);
            if (Random.Range(0.0f, 1.0f) <= CollectorBuyChance || (nbRecoltBuy==0 && currentPosCurrentZ>=(startPosZ+(levelLength/4))) && nbRecoltBuy<4) {

                GameObject obj = (GameObject)Instantiate(CollectorBuyPrefab, new Vector3(4.2f, 1, currentPosCurrentZ),Quaternion.identity);
                RecoltShop rs = obj.GetComponent<RecoltShop>();
                rs.priceCurve = new AnimationCurve();
                rs.priceCurve.AddKey(new Keyframe(0, CollectorCostMinOverLength.Evaluate(lengthRatio)));
                rs.priceCurve.AddKey(new Keyframe(1, CollectorCostMaxOverLength.Evaluate(lengthRatio)));

                rs.currentPrice = Mathf.RoundToInt(rs.priceCurve.Evaluate(rs.currentBuy / rs.maxBuy-1));
                rs.counter.text = rs.amountGiven + " / " + rs.currentPrice;
                spawnedElement.Add(obj);
                nbRecoltBuy++;
                currentPosCurrentZ += 2;
            }else
            if (Random.Range(0.0f, 1.0f) <= BuffBuyChance || (nbBuffBuy == 0 && currentPosCurrentZ >= (startPosZ + (levelLength / 4))) && nbBuffBuy < 4)
            {

                GameObject obj = (GameObject)Instantiate(BuffBuyPrefab, new Vector3(4.2f, 1, currentPosCurrentZ), Quaternion.identity);
                BuffShop rs = obj.GetComponent<BuffShop>();
                rs.priceCurve = new AnimationCurve();
                rs.priceCurve.AddKey(new Keyframe(0, BuffCostMinOverLength.Evaluate(lengthRatio)));
                rs.priceCurve.AddKey(new Keyframe(1, BuffCostMaxOverLength.Evaluate(lengthRatio)));
                rs.currentPrice = Mathf.RoundToInt(rs.priceCurve.Evaluate(rs.currentBuy / rs.maxBuy - 1));
                rs.counter.text = rs.amountGiven + " / " + rs.currentPrice;
                spawnedElement.Add(obj);
                nbBuffBuy++;
                currentPosCurrentZ += 2;
            }
            else
            {
                int RessourcesAfterForest = 0;
                int nbOfTreePerRow = levelWidth / 2;
                int forestSize = (int)Random.Range(1, forestLengthOverLenght.Evaluate(lengthRatio)) ;//Random.Range(1, 5);

                for (int i = 0; i < forestSize; i++)
                {
                    for (int j = 0; j < nbOfTreePerRow; j++)
                    {
                        GameObject obj = (GameObject)Instantiate(TreePrefab, new Vector3(-nbOfTreePerRow + (2 * (j + 0.5f)), 1, currentPosCurrentZ), Quaternion.identity);
                        Ressource re = obj.GetComponent<Ressource>();
                        re.startLifePoint = (int)TreeLifeOverLenght.Evaluate(lengthRatio);
                        re.RessourceOnBreak = (int)TreeRessourceOverLenght.Evaluate(lengthRatio);
                        RessourcesAfterForest += re.RessourceOnBreak;
                        spawnedElement.Add(obj);
                    }
                    currentPosCurrentZ += 2;
                }
            }
            currentPosCurrentZ += 5;

        }



        


        



    }






}
