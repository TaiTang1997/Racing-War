using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolOnline : MonoBehaviour
{
    public static ObjectPoolOnline Instance;
    [SerializeField]
    public Dictionary<PoolIDOnline, List<GameObject>> poolers;
    public List<PoolItemOnline> items;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        poolers = new Dictionary<PoolIDOnline, List<GameObject>>();
    }

    #region Khởi tạo obj
    // Khi muốn tạo 1 obj thì dùng ObjectPooler.ins.GetObject(id)
    public GameObject GetObject(PoolIDOnline id)
    {
        GameObject obj = null;

        // Kiểm tra xem trong pool đã có key là id chưa. Nếu chưa thì khởi tạo key và tạo obj
        if (!poolers.ContainsKey(id))
        {
            poolers.Add(id, new List<GameObject>());
            obj = Instantiate(GetPrefabFromID(id));
            poolers[id].Add(obj);
            //obj.transform.SetParent(transform);
            return obj;
        }
        // Kiểm tra pool đó có item có id này và đang inActive thì trả về obj đó
        foreach (var item in poolers[id])
        {
            if (!item.activeInHierarchy)
            {
                obj = item;
                //obj.SetActive(true);
                return obj;
            }
        }
        // Nếu không có item nào đang inActive thì tạo 1 obj mới
        obj = Instantiate(GetPrefabFromID(id));
        poolers[id].Add(obj);
        return obj;
    }


    // Lấy ra prefab theo id
    public GameObject GetPrefabFromID(PoolIDOnline id)
    {
        foreach (var item in items)
        {
            if (id == item.id)
                return item.prefab;
        }
        return null;
    }
    #endregion

    #region Return Obj về pool
    public void ReturnPool(GameObject obj, float time)
    {
        if (obj != null)
        {
            StartCoroutine(Return(obj, time));
        }
    }

    // sau khoang thoi gian time thi tat di
    IEnumerator Return(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
        {
            obj.SetActive(false);
        }

    }

    #endregion
    // Tùy đối tượng cần cho vào pool thì m cho 1 id tương ứng nhé. Có thể là đạn, effect, enemy

    // Lớp chứa các đối tượng cần cho vào pool

}
[System.Serializable]
public class PoolItemOnline
{
    public PoolIDOnline id; // Id của obj
    public GameObject prefab;  // Prefab của obj cần pool
}
public enum PoolIDOnline
{
    None = 0,
    Ferrari,
    Ip,
    Saleen,
    Smart,
    Nissan,
    Tt,
    FerrariWpOnline,
    FerrariWpOnline2,
}
