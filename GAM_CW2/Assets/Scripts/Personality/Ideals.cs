using System;

[Serializable]
//! JSON-able object to pass to server to submit Ideal personality traits
public class Ideals {

    public string id;
    public float openess;
    public float conscientiousness;
    public float extraversion;
    public float agreeableness;
    public float emotionalrange;

    //! Basic Constructor
    public Ideals(string _id, float _openess, float _conscientiousness, float _extraversion, float _agreeableness, float _emotionalrange)
    {
        id = _id;
        openess = _openess;
        conscientiousness = _conscientiousness;
        extraversion = _extraversion;
        agreeableness = _agreeableness;
        emotionalrange = _emotionalrange;
    }
}
