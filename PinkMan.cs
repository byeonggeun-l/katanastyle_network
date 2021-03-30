using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkMan : Character
{
    public GameObject prefabApple;

    GameObject attackedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        pAnimator = GetComponent<Animator>();

        startingHealth = 100.0f;
        health = startingHealth;

        characterType = CharacterType.Monster;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Lazer" ||
            collision.gameObject.tag == "Slash")
        {
            if (collision.GetComponent<AttackType>().player == gameObject)
                return;

            Collisioned(collision);
            pAnimator.SetTrigger("Attakced");

            Invoke("Attack", 1.0f);
            // Attack 함수에서 사용하기 위해.
            attackedPlayer = collision.GetComponent<AttackType>().player;
            // 레이저는 충돌 즉시 사라진다.
            if (collision.gameObject.tag == "Lazer")
                collision.gameObject.SetActive(false);
        }
        return;
    }

    private void Attack()
    {
        // 죽었는데 공격하는 것을 방지한다.
        if (dead)
            return;
        pAnimator.SetTrigger("Attack");
        Hit_LazerManager.instance.GetHit_Lazer(gameObject, attackedPlayer.transform.position);
    }


    private void Collisioned(Collider2D collision)
    {
        if (collision.GetComponent<AttackType>().player.GetComponent<Player>().dead)
            return;

        AddForce(collision);
        float Damage = collision.GetComponent<AttackType>().player.GetComponent<Character>().damage;

        bool Die = OnDamage(Damage);
        if (Die)
        {
            collision.GetComponent<AttackType>().player.GetComponent<Player>().SetScore(3);
            // 몬스터가 사망할 경우 그 자리에 Apple 을 생성한다.
            Instantiate(prefabApple, transform.position, Quaternion.identity);
        }
    }
}
