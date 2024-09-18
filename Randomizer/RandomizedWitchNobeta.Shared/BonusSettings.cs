using System.Text.Json.Serialization;

namespace RandomizedWitchNobeta.Shared;

public class BonusSettings
{
    public enum GameSkin
    {
        Witch,
        Necromancer,
        Vampire,
        Maid,
        SailorSuit,
        Swimsuit,
        BearMuppet,
        Bunny,
        MidnightKitty,
        ChineseDress,
        Nurse,
        Tania,
        Monica,
        Vanessa,
        BlackCat,
        DragonPrincess,
        KnittedUniform,
        LandMineGirl
    }

    public enum RandomSkin
    {
        Never,
        Once,
        Always
    }

    public GameSkin SelectedSkin { get; init; } = GameSkin.Witch;

    public RandomSkin RandomizeSkin { get; init; } = RandomSkin.Never;

    public bool HideBag { get; init; } = false;

    public bool HideStaff { get; init; } = false;

    public bool HideHat { get; init; } = false;
}