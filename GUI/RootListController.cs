using System;

namespace SyncTwoCo
{
  /// <summary>
  /// Summary description for RootListController.
  /// </summary>
  public class RootListController
  {
    MainDocument _document;
    RootList _view;

    public RootList View { get { return _view; }}

    public RootListController(MainDocument document)
    {
      _document = document;

      _document.Align();


      _view = new RootList();
      _view._controller = this;

      _view.InitializeListCaptions(_document.MyRootComputerName,_document.ForeignRootComputerName);
      
      UpdateList();
    
    }

    public void UpdateList()
    {
      string[] myRoots = new string[_document.Count];
      for(int i=0;i<_document.Count;i++)
        myRoots[i]= _document.MyRoot(i)==null ? null : _document.MyRoot(i).FilePath;


      string[] foRoots = new string[_document.Count];
      for(int i=0;i<_document.Count;i++)
        foRoots[i]=_document.ForeignRoot(i)==null ? null :_document.ForeignRoot(i).FilePath;


      _view.InitializeList(myRoots,foRoots);

    }

    public void EhView_ItemDoubleClick(int item)
    {
     
      if(_document.MyRoot(item).IsValid)
      {
      }
      else
      {
        System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
        if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
        {
          _document.SetBasePath(item,dlg.SelectedPath);
          UpdateList();
        }
      }
      
    }

    public void EhView_BeforeLabelEdit(int item)
    {
      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        _document.SetBasePath(item,dlg.SelectedPath);
        UpdateList();
      }
    }

    public void EhView_AddPath()
    {
     
      System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
      if(System.Windows.Forms.DialogResult.OK==dlg.ShowDialog(Current.MainForm))
      {
        _document.AddRoot(dlg.SelectedPath);
        UpdateList();
      }
    }
      
    public void EhView_EditFilterList(int item)
    {
      FilterListItemListController controller = new FilterListItemListController(
        this._document.RootPair(item).PathFilter,
        this._document.RootPair(item).MyRoot.FilePath,
        this._document.RootPair(item).ForeignRoot.FilePath);
      FilterListItemListControl control = new FilterListItemListControl();
      controller.View = control;
      DialogShellController dlgctrl = new DialogShellController(new DialogShellView(control),controller);
      dlgctrl.ShowDialog(this.View.ParentForm);

    }
   
  }
}
