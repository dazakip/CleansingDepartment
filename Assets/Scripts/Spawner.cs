using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject rubbishPrefab;
    public GameObject indicatorPrefab;

    public GameObject beltBellySprite;

    private float spawnBoundLeft;
    private float spawnBoundRight;
    private float spawnBoundTop;
    private float spawnBoundBottom;

    float elapsed = 0f;
    float spawnTime = 2f;
    int spriteLayer = 5;

    void Awake()
    {
        SpriteRenderer beltSprite = beltBellySprite.GetComponent<SpriteRenderer>();

        // spawn rubbish in this square
        spawnBoundLeft = -beltSprite.bounds.extents.x * 2;
        spawnBoundRight = -beltSprite.bounds.extents.x;
        spawnBoundTop = beltBellySprite.transform.position.y + beltSprite.bounds.extents.y;
        spawnBoundBottom = beltBellySprite.transform.position.y - beltSprite.bounds.extents.y;

        GameManager.Instance.BarrierBottomY = spawnBoundBottom;
        GameManager.Instance.BarrierTopY = spawnBoundTop;
    }

    void Update()
    {
        if (GameManager.Instance.On)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= spawnTime)
            {
                elapsed = elapsed % 1f;
                SpawnRubbish();
            }
        }
    }

    private void SpawnRubbish()
    {
        for (int i = 0; i <= 10; i++)
        {
            IncrementSpriteLayer();
            switch (Random.Range(0, 5))
            {
                case 0:
                    CreateRubbishItem("Glass", 13);
                    break;
                case 1:
                    CreateRubbishItem("Paper", 26);
                    break;
                case 2:
                    CreateRubbishItem("Tin", 21);
                    break;
                case 3:
                    CreateRubbishItem("Plastic", 24);
                    break;
                case 4:
                    CreateRubbishItem("Non–Recyclable", 12);
                    break;
            }
        }
    }

    private void IncrementSpriteLayer()
    {
        spriteLayer += 1;
        if (spriteLayer > 100)
        {
            spriteLayer = 5;
        }
    }

    private void CreateRubbishItem(string type, int noOfSprites)
    {
        int randomSprite = Random.Range(1, noOfSprites);
        Sprite sprite = Resources.Load<Sprite>($"RubbishSprites/{type}/{randomSprite}");

        var randomX = Random.Range(spawnBoundLeft - 10, spawnBoundRight);
        var randomY = Random.Range(spawnBoundBottom, spawnBoundTop);

        var newItem = Instantiate(rubbishPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity, transform);

        var randomAngle = Random.Range(-75, 75);
        newItem.transform.rotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        newItem.GetComponent<SpriteRenderer>().sortingOrder = spriteLayer;
        newItem.GetComponent<SpriteRenderer>().sprite = sprite;
        newItem.AddComponent<BoxCollider2D>();
        newItem.GetComponent<BoxCollider2D>().isTrigger = true;

        if (type.Equals("Non–Recyclable"))
        {
            newItem.GetComponent<RubbishItem>().isBadRubbish = true;
            Instantiate(indicatorPrefab, newItem.transform);
        }
    }
}
