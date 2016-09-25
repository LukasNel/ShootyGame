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
        gridSize = (int)(this.transform.localScale.x/blockSize);
        grid = DungeonGenerationDrunkenWalk(gridSize, iterations, minArea, false, true);

        int tI = 0;
        GameObject curCube;
        Vector3 startPos = this.transform.position - new Vector3(this.transform.localScale.x / 2 - blockSize / 2, 0, this.transform.localScale.z / 2 - blockSize / 2);
        NavMeshObstacle navMeshObstacle;
        Debug.Log(gridSize);

        PrintDungeon(grid, gridSize);
        int count = 0;
        startPos = this.transform.position - 0.5f *
            (new Vector3(this.transform.localScale.x - blockSize, 0, this.transform.localScale.z - blockSize));
        for (int i = 0; i < gridSize; i++) {
            grid[i, gridSize - 1] = false;
            grid[gridSize - 1,i]  = false;
            grid[i, 0]            = false;
            grid[0, i]            = false;
        }
        
        //The order of variables is correct, DO NOT CHANGE, otherwise this code "i += tI" won;t work.
        for (int j = 0; j < gridSize; j++) { 
            for (int i = 0;i < gridSize;i++)
            {
                if (grid[i,j] == false) {
                    count++;
                    tI = 0;
                   while (i + tI < gridSize && grid[i + tI, j] == false)
                    {

                      //    grid[i + tI, j] = true;

                        tI++;
                    }
                    tI--;
                    curCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    curCube.transform.position = startPos + new Vector3((i+(((float)tI)/2))*blockSize,0, j*blockSize);
                    curCube.transform.localScale = new Vector3(tI * blockSize + blockSize,blockHeight,blockSize);
                    /*
                    navMeshObstacle = curCube.AddComponent<NavMeshObstacle>();
                    navMeshObstacle.carving = true;
                    navMeshObstacle.carveOnlyStationary = true;
                    navMeshObstacle.shape = NavMeshObstacleShape.Box;
                    */
                    
                   i += tI;
                    Debug.Log(i);
                }
            }
            //Debug.Log(i + " + " + tI);

        }
        PrintDungeon(grid, gridSize);
        Debug.Log(count);
        Debug.Log(gridSize * gridSize);
        Debug.Log(minArea);
    }
    void PrintDungeon(bool[,] dungeon, int fgridsize) {
        string tstr = "";
        for (int i = 0; i < fgridsize; i++)
        {
            for (int j = 0; j < fgridsize; j++)
            {
                tstr += (dungeon[i, j]) ? "1" : " ";
            }
            tstr += "\n";
        }
        Debug.Log(tstr);
    }
    bool[,] DungeonGenerationDrunkenWalk(int fgridSize,int fiterations,int fminArea,bool wrap,bool noDiagonal) {
        Debug.Log(fgridSize);

        bool[,] fgrid = new bool[fgridSize, fgridSize];
        int curArea = 0;
        int fx = fgridSize / 2;
        int fy = fgridSize / 2;
        //fx = Random.Range(0, fgridSize);
        //fy = Random.Range(0, fgridSize);
        for (int i = 0; i < fgridSize; i++) for (int j = 0; j < fgridSize; j++) fgrid[i, j] = false;

        for (int i = 0; i < fiterations || curArea < fminArea; i++)
        {
            if (fgrid[fx, fy] == false) curArea++;

            fgrid[fx, fy] = true;
       
            if (noDiagonal)
            {
                if (Random.Range(0, 2) == 1)
                {
                    fx += (Random.Range(0, 2) == 1) ? -1 : 1; //Random.Range(-1, 2);
                    //fx += Random.Range(-1, 2);
                }
                else {
                    //fy += Random.Range(-1, 2);
                    fy += (Random.Range(0, 2) == 1) ? -1 : 1;
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
        minArea = curArea;
        return fgrid;

    }
	// Update is called once per frame
	void Update () {
	
	}
}
