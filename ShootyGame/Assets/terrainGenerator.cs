using UnityEngine;
using System.Collections;

public class terrainGenerator : MonoBehaviour {
    public float blockHeight = 0.0f;
    public float blockSize = 2.0f;
    public int iterations = 1200;
    public int minArea = 900;
    bool[,] grid;

    int gridSize = 0;
    // Use this for initialization
    void Start()
    {
        gridSize = (int)(Mathf.Max(this.transform.localScale.x, this.transform.localScale.z)/blockSize);
        grid = DungeonGenerationDrunkenWalk(gridSize, iterations, minArea, false, true);

        int tI = 0;
        GameObject curCube;
        Vector3 startPos = this.transform.position - new Vector3(this.transform.localScale.x/2 - blockSize/2,0, this.transform.localScale.z/2 - blockSize / 2);
        NavMeshObstacle navMeshObstacle;
        Debug.Log(gridSize);
        //The order of variables is correct, DO NOT CHANGE, otherwise this code "i += tI" won;t work.
        for (int j = 0; j < gridSize; j++) { 
            for (int i = 0; i < gridSize;i++)
            {
                if (!grid[i,j]) {
                    tI = 0;
                    while (i + tI < gridSize && grid[i + tI, j] == false)tI++;
                    tI--;
                    curCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    curCube.transform.position = startPos + new Vector3((float)i+(((float)tI)/2)*blockSize,0, j*blockSize);
                    curCube.transform.localScale = new Vector3((((float)tI) / 2) * blockSize,blockHeight,blockSize);

                    navMeshObstacle = curCube.AddComponent<NavMeshObstacle>();
                    navMeshObstacle.carving = true;
                    navMeshObstacle.carveOnlyStationary = true;
                    navMeshObstacle.shape = NavMeshObstacleShape.Box;

                    i += tI;            
                }
            }
            
        }
    }
    bool[,] DungeonGenerationDrunkenWalk(int fgridSize,int fiterations,int fminArea,bool wrap,bool noDiagonal) {
        Debug.Log(fgridSize);

        bool[,] fgrid = new bool[fgridSize, fgridSize];
        int curArea = 0;
        int fx = fgridSize / 2;
        int fy = fgridSize / 2;

        for (int i = 0; i < fgridSize; i++) for (int j = 0; j < fgridSize; j++) fgrid[i, j] = false;

        for (int i = 0; i < fiterations || curArea < fminArea; i++)
        {
            if (fgrid[fx, fy] == false) curArea++;

            fgrid[fx, fy] = true;
       
            if (noDiagonal)
            {
                if (Random.Range(0, 2) == 1)
                {
                    fx += Random.Range(-1, 2);
                }
                else {
                    fy += Random.Range(-1, 2);
                }
            }
            else {
                fx += Random.Range(-1, 2);
                fy += Random.Range(-1, 2);
            }

            if (wrap)
            {
                fx = fx % fgridSize;
                fy = fy % fgridSize;
                fx = (fx < 0) ? fgridSize - 1 : fx;
                fy = (fy < 0) ? fgridSize - 1 : fy;
            }
            else
            {
                fx = Mathf.Min(fx, fgridSize - 1);
                fy = Mathf.Min(fy, fgridSize - 1);
                fx = Mathf.Max(fx, 0);
                fy = Mathf.Max(fy, 0);
            }
        }

        return fgrid;

    }
	// Update is called once per frame
	void Update () {
	
	}
}
