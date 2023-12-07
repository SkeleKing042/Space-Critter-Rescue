//Created by Jackson Lucas
//Last edited by Jackson Lucas

using UnityEngine;
using UnityEngine.AI;

public class CreatureStats : MonoBehaviour
{
    [Header("Creature Info")]
    [Tooltip("Whether or not this is a large creature.")]
    public bool IsBig;
    public enum creatureType
    {
        Shroom,
        Crystal
    };
    [Tooltip("What type of creature this one is.")]
    public creatureType Type;

    [Header("Threasholds")]
    [SerializeField, Range(0f, 100f), Tooltip("Point at which this creature will look for water.")]
    private float _thirstiness;
    [SerializeField, Range(0f, 100f), Tooltip("How quickly this creature will rest.")]
    private float _lazyness;
    [SerializeField, Tooltip("The velocity of a rigidbody needs to be moving on impact to cause this creature to panic.")]
    private float _panicVelocity;

    [Header("Navigation")]
    [Tooltip("The maximum distance that a creature will travel from their initial starting position.")]
    public float ExplorationRange;
    [SerializeField, Tooltip("The speed the creature has while panicing")]
    private float _panicSpeed;


    private CreatureAI _ai;

    public void GetStats()
    {
        _ai = GetComponent<CreatureAI>();

        _ai.InitStats(_thirstiness, _lazyness, ExplorationRange, _panicVelocity, _panicSpeed);
    }
}
