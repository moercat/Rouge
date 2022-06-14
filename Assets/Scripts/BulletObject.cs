using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{
    public float FiringSpeed;
    public float Damage;

    public bool IsPlayerBullet;

    public void Initialize(float firingSpeed, float damage){

        FiringSpeed = firingSpeed;
        Damage = damage;

    }
    // 子弹判断是敌人还是玩家射出的子弹
    public void Start(){
        
        if(this.transform.parent.gameObject.CompareTag("Player")){
            IsPlayerBullet = true;
        } else if (this.transform.parent.gameObject.CompareTag("Enemy")){
            IsPlayerBullet = false;
        }
        this.transform.SetParent(null);
        StartCoroutine(DeathTimer());

    }
    
    /*
      OnCollisionEnter方法被触发要符合以下条件
    1 碰撞双方必须是碰撞体 
    2 碰撞的主动方必须是刚体，注意我的用词是主动方，而不是被动方 
    3 刚体不能勾选IsKinematic 
    4 碰撞体不能够勾选IsTigger

    OnCollisionEnter方法的形参对象指的是碰撞双方中没有携带OnCollisionEnter方法的一方
     */
    public void OnCollisionEnter(Collision col){
        if(IsPlayerBullet){
            // 命中敌人
            if (col.gameObject.CompareTag("Enemy")){
                // 敌人扣血
                col.gameObject.GetComponent<AIController>().DeltaHealth(-Damage);
                Destroy(this.gameObject);
            } else if (col.gameObject.CompareTag("Finish")){
                Destroy(this.gameObject);
            }
        } else if(!IsPlayerBullet){
            // 命中玩家
            if (col.gameObject.CompareTag("Player")){
                // 玩家扣血
                col.gameObject.GetComponent<PlayerController>().DeltaHealth(-Damage);
                // 玩家扣血应该也需要清除子弹
                // Destroy(this.gameObject);
            } else if (col.gameObject.CompareTag("Finish")){
                Destroy(this.gameObject);
            }
        }
    }

    // 协程清子弹
    public IEnumerator DeathTimer(){
        float time = 2f;
        float totalTime = 0f;
        while(totalTime < time){
            totalTime += Time.deltaTime;
            yield return null;
        }
        // TODO 需考虑对象池
        Destroy(this.gameObject);
    }

    // 未完成函数？
	// void Jump() {
	// 	if speed
	//
	// }

}
