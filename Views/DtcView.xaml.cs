using System.Collections.ObjectModel;

namespace Eagle.Views;

public partial class DtcView : ContentView
{
    public ObservableCollection<DtcItem> DtcList { get; set; } = new();

    public DtcView()
    {
        InitializeComponent();
        DtcCollectionView.ItemsSource = DtcList;
    }

    public void AddDtc(string code, string description)
    {
        DtcList.Add(new DtcItem { Code = code, Description = description });
    }

    public void ClearDtc()
    {
        DtcList.Clear();
    }
}
public class DtcItem
{
    public string Code { get; set; }
    public string Description { get; set; }
}
