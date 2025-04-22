namespace Umove_AS.UI
{
    /// <summary> 
    /// MenuItem er gør at der gemmes på stacken.
    /// Readonly. 
    /// Den bliver brugt i forhold til kald af menuer i Program.
    /// </summary>
    public readonly record struct MenuItem(string Text, Action Action);
    // Action, peger på en metode, uden at man kalder, med det samme. 
    // record struct er en letvægtstype, der gemmes på
    // stacken og automatisk sammenlignes på indhold i stedet for reference. 
    // Den er perfekt til små, uforanderlige datatyper(som menupunkter),
    // fordi den kræver minimal kode og er hurtig i brug.
}