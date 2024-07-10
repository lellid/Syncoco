#region Copyright
/////////////////////////////////////////////////////////////////////////////
//    Syncoco: offline file synchronization
//    Copyright (C) 2004-2099 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////
#endregion

namespace Syncoco.GUI
{
  /// <summary>
  /// Summary description for AboutDialog.
  /// </summary>
  public class AboutDialog : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Button m_btOK;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.LinkLabel m_LinkLabel;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public AboutDialog()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      // Create a new link using the Add method of the LinkCollection class.
      int len = m_LinkLabel.Text.Length;
      int pos = m_LinkLabel.Text.IndexOf("http://");
      m_LinkLabel.Links.Add(pos, len - pos, "http://sourceforge.net/projects/syncoco");


    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutDialog));
      this.m_btOK = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.m_LinkLabel = new System.Windows.Forms.LinkLabel();
      this.SuspendLayout();
      // 
      // m_btOK
      // 
      this.m_btOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.m_btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.m_btOK.Location = new System.Drawing.Point(216, 432);
      this.m_btOK.Name = "m_btOK";
      this.m_btOK.TabIndex = 0;
      this.m_btOK.Text = "OK";
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label1.Location = new System.Drawing.Point(168, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(128, 40);
      this.label1.TabIndex = 1;
      this.label1.Text = "Syncoco";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label2.Location = new System.Drawing.Point(64, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(432, 32);
      this.label2.TabIndex = 2;
      this.label2.Text = "synchronization by transfer medium";
      // 
      // textBox1
      // 
      this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBox1.Location = new System.Drawing.Point(8, 112);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBox1.Size = new System.Drawing.Size(496, 312);
      this.textBox1.TabIndex = 3;
      this.textBox1.Text = "\t\t    ACKNOWLEDGEMENTS\r\n\r\nI want to thank my wife Ellen for for her support and p" +
        "atience.\r\n                 Dr. D. Lellinger\r\n\r\n\r\n\t\t    GNU GENERAL PUBLIC LICENS" +
        "E\r\n\t\t       Version 2, June 1991\r\n\r\n Copyright (C) 1989, 1991 Free Software Foun" +
        "dation, Inc.\r\n                       59 Temple Place, Suite 330, Boston, MA  021" +
        "11-1307  USA\r\n Everyone is permitted to copy and distribute verbatim copies\r\n of" +
        " this license document, but changing it is not allowed.\r\n\r\n\t\t\t    Preamble\r\n\r\n  " +
        "The licenses for most software are designed to take away your\r\nfreedom to share " +
        "and change it.  By contrast, the GNU General Public\r\nLicense is intended to guar" +
        "antee your freedom to share and change free\r\nsoftware--to make sure the software" +
        " is free for all its users.  This\r\nGeneral Public License applies to most of the" +
        " Free Software\r\nFoundation\'s software and to any other program whose authors com" +
        "mit to\r\nusing it.  (Some other Free Software Foundation software is covered by\r\n" +
        "the GNU Library General Public License instead.)  You can apply it to\r\nyour prog" +
        "rams, too.\r\n\r\n  When we speak of free software, we are referring to freedom, not" +
        "\r\nprice.  Our General Public Licenses are designed to make sure that you\r\nhave t" +
        "he freedom to distribute copies of free software (and charge for\r\nthis service i" +
        "f you wish), that you receive source code or can get it\r\nif you want it, that yo" +
        "u can change the software or use pieces of it\r\nin new free programs; and that yo" +
        "u know you can do these things.\r\n\r\n  To protect your rights, we need to make res" +
        "trictions that forbid\r\nanyone to deny you these rights or to ask you to surrende" +
        "r the rights.\r\nThese restrictions translate to certain responsibilities for you " +
        "if you\r\ndistribute copies of the software, or if you modify it.\r\n\r\n  For example" +
        ", if you distribute copies of such a program, whether\r\ngratis or for a fee, you " +
        "must give the recipients all the rights that\r\nyou have.  You must make sure that" +
        " they, too, receive or can get the\r\nsource code.  And you must show them these t" +
        "erms so they know their\r\nrights.\r\n\r\n  We protect your rights with two steps: (1)" +
        " copyright the software, and\r\n(2) offer you this license which gives you legal p" +
        "ermission to copy,\r\ndistribute and/or modify the software.\r\n\r\n  Also, for each a" +
        "uthor\'s protection and ours, we want to make certain\r\nthat everyone understands " +
        "that there is no warranty for this free\r\nsoftware.  If the software is modified " +
        "by someone else and passed on, we\r\nwant its recipients to know that what they ha" +
        "ve is not the original, so\r\nthat any problems introduced by others will not refl" +
        "ect on the original\r\nauthors\' reputations.\r\n\r\n  Finally, any free program is thr" +
        "eatened constantly by software\r\npatents.  We wish to avoid the danger that redis" +
        "tributors of a free\r\nprogram will individually obtain patent licenses, in effect" +
        " making the\r\nprogram proprietary.  To prevent this, we have made it clear that a" +
        "ny\r\npatent must be licensed for everyone\'s free use or not licensed at all.\r\n\r\n " +
        " The precise terms and conditions for copying, distribution and\r\nmodification fo" +
        "llow.\r\n\r\n\t\t    GNU GENERAL PUBLIC LICENSE\r\n   TERMS AND CONDITIONS FOR COPYING, " +
        "DISTRIBUTION AND MODIFICATION\r\n\r\n  0. This License applies to any program or oth" +
        "er work which contains\r\na notice placed by the copyright holder saying it may be" +
        " distributed\r\nunder the terms of this General Public License.  The \"Program\", be" +
        "low,\r\nrefers to any such program or work, and a \"work based on the Program\"\r\nmea" +
        "ns either the Program or any derivative work under copyright law:\r\nthat is to sa" +
        "y, a work containing the Program or a portion of it,\r\neither verbatim or with mo" +
        "difications and/or translated into another\r\nlanguage.  (Hereinafter, translation" +
        " is included without limitation in\r\nthe term \"modification\".)  Each licensee is " +
        "addressed as \"you\".\r\n\r\nActivities other than copying, distribution and modificat" +
        "ion are not\r\ncovered by this License; they are outside its scope.  The act of\r\nr" +
        "unning the Program is not restricted, and the output from the Program\r\nis covere" +
        "d only if its contents constitute a work based on the\r\nProgram (independent of h" +
        "aving been made by running the Program).\r\nWhether that is true depends on what t" +
        "he Program does.\r\n\r\n  1. You may copy and distribute verbatim copies of the Prog" +
        "ram\'s\r\nsource code as you receive it, in any medium, provided that you\r\nconspicu" +
        "ously and appropriately publish on each copy an appropriate\r\ncopyright notice an" +
        "d disclaimer of warranty; keep intact all the\r\nnotices that refer to this Licens" +
        "e and to the absence of any warranty;\r\nand give any other recipients of the Prog" +
        "ram a copy of this License\r\nalong with the Program.\r\n\r\nYou may charge a fee for " +
        "the physical act of transferring a copy, and\r\nyou may at your option offer warra" +
        "nty protection in exchange for a fee.\r\n\r\n  2. You may modify your copy or copies" +
        " of the Program or any portion\r\nof it, thus forming a work based on the Program," +
        " and copy and\r\ndistribute such modifications or work under the terms of Section " +
        "1\r\nabove, provided that you also meet all of these conditions:\r\n\r\n    a) You mus" +
        "t cause the modified files to carry prominent notices\r\n    stating that you chan" +
        "ged the files and the date of any change.\r\n\r\n    b) You must cause any work that" +
        " you distribute or publish, that in\r\n    whole or in part contains or is derived" +
        " from the Program or any\r\n    part thereof, to be licensed as a whole at no char" +
        "ge to all third\r\n    parties under the terms of this License.\r\n\r\n    c) If the m" +
        "odified program normally reads commands interactively\r\n    when run, you must ca" +
        "use it, when started running for such\r\n    interactive use in the most ordinary " +
        "way, to print or display an\r\n    announcement including an appropriate copyright" +
        " notice and a\r\n    notice that there is no warranty (or else, saying that you pr" +
        "ovide\r\n    a warranty) and that users may redistribute the program under\r\n    th" +
        "ese conditions, and telling the user how to view a copy of this\r\n    License.  (" +
        "Exception: if the Program itself is interactive but\r\n    does not normally print" +
        " such an announcement, your work based on\r\n    the Program is not required to pr" +
        "int an announcement.)\r\n\r\nThese requirements apply to the modified work as a whol" +
        "e.  If\r\nidentifiable sections of that work are not derived from the Program,\r\nan" +
        "d can be reasonably considered independent and separate works in\r\nthemselves, th" +
        "en this License, and its terms, do not apply to those\r\nsections when you distrib" +
        "ute them as separate works.  But when you\r\ndistribute the same sections as part " +
        "of a whole which is a work based\r\non the Program, the distribution of the whole " +
        "must be on the terms of\r\nthis License, whose permissions for other licensees ext" +
        "end to the\r\nentire whole, and thus to each and every part regardless of who wrot" +
        "e it.\r\n\r\nThus, it is not the intent of this section to claim rights or contest\r\n" +
        "your rights to work written entirely by you; rather, the intent is to\r\nexercise " +
        "the right to control the distribution of derivative or\r\ncollective works based o" +
        "n the Program.\r\n\r\nIn addition, mere aggregation of another work not based on the" +
        " Program\r\nwith the Program (or with a work based on the Program) on a volume of\r" +
        "\na storage or distribution medium does not bring the other work under\r\nthe scope" +
        " of this License.\r\n\r\n  3. You may copy and distribute the Program (or a work bas" +
        "ed on it,\r\nunder Section 2) in object code or executable form under the terms of" +
        "\r\nSections 1 and 2 above provided that you also do one of the following:\r\n\r\n    " +
        "a) Accompany it with the complete corresponding machine-readable\r\n    source cod" +
        "e, which must be distributed under the terms of Sections\r\n    1 and 2 above on a" +
        " medium customarily used for software interchange; or,\r\n\r\n    b) Accompany it wi" +
        "th a written offer, valid for at least three\r\n    years, to give any third party" +
        ", for a charge no more than your\r\n    cost of physically performing source distr" +
        "ibution, a complete\r\n    machine-readable copy of the corresponding source code," +
        " to be\r\n    distributed under the terms of Sections 1 and 2 above on a medium\r\n " +
        "   customarily used for software interchange; or,\r\n\r\n    c) Accompany it with th" +
        "e information you received as to the offer\r\n    to distribute corresponding sour" +
        "ce code.  (This alternative is\r\n    allowed only for noncommercial distribution " +
        "and only if you\r\n    received the program in object code or executable form with" +
        " such\r\n    an offer, in accord with Subsection b above.)\r\n\r\nThe source code for " +
        "a work means the preferred form of the work for\r\nmaking modifications to it.  Fo" +
        "r an executable work, complete source\r\ncode means all the source code for all mo" +
        "dules it contains, plus any\r\nassociated interface definition files, plus the scr" +
        "ipts used to\r\ncontrol compilation and installation of the executable.  However, " +
        "as a\r\nspecial exception, the source code distributed need not include\r\nanything " +
        "that is normally distributed (in either source or binary\r\nform) with the major c" +
        "omponents (compiler, kernel, and so on) of the\r\noperating system on which the ex" +
        "ecutable runs, unless that component\r\nitself accompanies the executable.\r\n\r\nIf d" +
        "istribution of executable or object code is made by offering\r\naccess to copy fro" +
        "m a designated place, then offering equivalent\r\naccess to copy the source code f" +
        "rom the same place counts as\r\ndistribution of the source code, even though third" +
        " parties are not\r\ncompelled to copy the source along with the object code.\r\n\r\n  " +
        "4. You may not copy, modify, sublicense, or distribute the Program\r\nexcept as ex" +
        "pressly provided under this License.  Any attempt\r\notherwise to copy, modify, su" +
        "blicense or distribute the Program is\r\nvoid, and will automatically terminate yo" +
        "ur rights under this License.\r\nHowever, parties who have received copies, or rig" +
        "hts, from you under\r\nthis License will not have their licenses terminated so lon" +
        "g as such\r\nparties remain in full compliance.\r\n\r\n  5. You are not required to ac" +
        "cept this License, since you have not\r\nsigned it.  However, nothing else grants " +
        "you permission to modify or\r\ndistribute the Program or its derivative works.  Th" +
        "ese actions are\r\nprohibited by law if you do not accept this License.  Therefore" +
        ", by\r\nmodifying or distributing the Program (or any work based on the\r\nProgram)," +
        " you indicate your acceptance of this License to do so, and\r\nall its terms and c" +
        "onditions for copying, distributing or modifying\r\nthe Program or works based on " +
        "it.\r\n\r\n  6. Each time you redistribute the Program (or any work based on the\r\nPr" +
        "ogram), the recipient automatically receives a license from the\r\noriginal licens" +
        "or to copy, distribute or modify the Program subject to\r\nthese terms and conditi" +
        "ons.  You may not impose any further\r\nrestrictions on the recipients\' exercise o" +
        "f the rights granted herein.\r\nYou are not responsible for enforcing compliance b" +
        "y third parties to\r\nthis License.\r\n\r\n  7. If, as a consequence of a court judgme" +
        "nt or allegation of patent\r\ninfringement or for any other reason (not limited to" +
        " patent issues),\r\nconditions are imposed on you (whether by court order, agreeme" +
        "nt or\r\notherwise) that contradict the conditions of this License, they do not\r\ne" +
        "xcuse you from the conditions of this License.  If you cannot\r\ndistribute so as " +
        "to satisfy simultaneously your obligations under this\r\nLicense and any other per" +
        "tinent obligations, then as a consequence you\r\nmay not distribute the Program at" +
        " all.  For example, if a patent\r\nlicense would not permit royalty-free redistrib" +
        "ution of the Program by\r\nall those who receive copies directly or indirectly thr" +
        "ough you, then\r\nthe only way you could satisfy both it and this License would be" +
        " to\r\nrefrain entirely from distribution of the Program.\r\n\r\nIf any portion of thi" +
        "s section is held invalid or unenforceable under\r\nany particular circumstance, t" +
        "he balance of the section is intended to\r\napply and the section as a whole is in" +
        "tended to apply in other\r\ncircumstances.\r\n\r\nIt is not the purpose of this sectio" +
        "n to induce you to infringe any\r\npatents or other property right claims or to co" +
        "ntest validity of any\r\nsuch claims; this section has the sole purpose of protect" +
        "ing the\r\nintegrity of the free software distribution system, which is\r\nimplement" +
        "ed by public license practices.  Many people have made\r\ngenerous contributions t" +
        "o the wide range of software distributed\r\nthrough that system in reliance on con" +
        "sistent application of that\r\nsystem; it is up to the author/donor to decide if h" +
        "e or she is willing\r\nto distribute software through any other system and a licen" +
        "see cannot\r\nimpose that choice.\r\n\r\nThis section is intended to make thoroughly c" +
        "lear what is believed to\r\nbe a consequence of the rest of this License.\r\n\r\n  8. " +
        "If the distribution and/or use of the Program is restricted in\r\ncertain countrie" +
        "s either by patents or by copyrighted interfaces, the\r\noriginal copyright holder" +
        " who places the Program under this License\r\nmay add an explicit geographical dis" +
        "tribution limitation excluding\r\nthose countries, so that distribution is permitt" +
        "ed only in or among\r\ncountries not thus excluded.  In such case, this License in" +
        "corporates\r\nthe limitation as if written in the body of this License.\r\n\r\n  9. Th" +
        "e Free Software Foundation may publish revised and/or new versions\r\nof the Gener" +
        "al Public License from time to time.  Such new versions will\r\nbe similar in spir" +
        "it to the present version, but may differ in detail to\r\naddress new problems or " +
        "concerns.\r\n\r\nEach version is given a distinguishing version number.  If the Prog" +
        "ram\r\nspecifies a version number of this License which applies to it and \"any\r\nla" +
        "ter version\", you have the option of following the terms and conditions\r\neither " +
        "of that version or of any later version published by the Free\r\nSoftware Foundati" +
        "on.  If the Program does not specify a version number of\r\nthis License, you may " +
        "choose any version ever published by the Free Software\r\nFoundation.\r\n\r\n  10. If " +
        "you wish to incorporate parts of the Program into other free\r\nprograms whose dis" +
        "tribution conditions are different, write to the author\r\nto ask for permission. " +
        " For software which is copyrighted by the Free\r\nSoftware Foundation, write to th" +
        "e Free Software Foundation; we sometimes\r\nmake exceptions for this.  Our decisio" +
        "n will be guided by the two goals\r\nof preserving the free status of all derivati" +
        "ves of our free software and\r\nof promoting the sharing and reuse of software gen" +
        "erally.\r\n\r\n\t\t\t    NO WARRANTY\r\n\r\n  11. BECAUSE THE PROGRAM IS LICENSED FREE OF C" +
        "HARGE, THERE IS NO WARRANTY\r\nFOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICA" +
        "BLE LAW.  EXCEPT WHEN\r\nOTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR " +
        "OTHER PARTIES\r\nPROVIDE THE PROGRAM \"AS IS\" WITHOUT WARRANTY OF ANY KIND, EITHER " +
        "EXPRESSED\r\nOR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF\r" +
        "\nMERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.  THE ENTIRE RISK AS\r\nTO T" +
        "HE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU.  SHOULD THE\r\nPROGRAM PROV" +
        "E DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY SERVICING,\r\nREPAIR OR CORRECTI" +
        "ON.\r\n\r\n  12. IN NO EVENT UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITI" +
        "NG\r\nWILL ANY COPYRIGHT HOLDER, OR ANY OTHER PARTY WHO MAY MODIFY AND/OR\r\nREDISTR" +
        "IBUTE THE PROGRAM AS PERMITTED ABOVE, BE LIABLE TO YOU FOR DAMAGES,\r\nINCLUDING A" +
        "NY GENERAL, SPECIAL, INCIDENTAL OR CONSEQUENTIAL DAMAGES ARISING\r\nOUT OF THE USE" +
        " OR INABILITY TO USE THE PROGRAM (INCLUDING BUT NOT LIMITED\r\nTO LOSS OF DATA OR " +
        "DATA BEING RENDERED INACCURATE OR LOSSES SUSTAINED BY\r\nYOU OR THIRD PARTIES OR A" +
        " FAILURE OF THE PROGRAM TO OPERATE WITH ANY OTHER\r\nPROGRAMS), EVEN IF SUCH HOLDE" +
        "R OR OTHER PARTY HAS BEEN ADVISED OF THE\r\nPOSSIBILITY OF SUCH DAMAGES.\r\n\r\n\t\t    " +
        " END OF TERMS AND CONDITIONS\r\n\r\n\t    How to Apply These Terms to Your New Progra" +
        "ms\r\n\r\n  If you develop a new program, and you want it to be of the greatest\r\npos" +
        "sible use to the public, the best way to achieve this is to make it\r\nfree softwa" +
        "re which everyone can redistribute and change under these terms.\r\n\r\n  To do so, " +
        "attach the following notices to the program.  It is safest\r\nto attach them to th" +
        "e start of each source file to most effectively\r\nconvey the exclusion of warrant" +
        "y; and each file should have at least\r\nthe \"copyright\" line and a pointer to whe" +
        "re the full notice is found.\r\n\r\n    <one line to give the program\'s name and a b" +
        "rief idea of what it does.>\r\n    Copyright (C) <year>  <name of author>\r\n\r\n    T" +
        "his program is free software; you can redistribute it and/or modify\r\n    it unde" +
        "r the terms of the GNU General Public License as published by\r\n    the Free Soft" +
        "ware Foundation; either version 2 of the License, or\r\n    (at your option) any l" +
        "ater version.\r\n\r\n    This program is distributed in the hope that it will be use" +
        "ful,\r\n    but WITHOUT ANY WARRANTY; without even the implied warranty of\r\n    ME" +
        "RCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the\r\n    GNU General Pub" +
        "lic License for more details.\r\n\r\n    You should have received a copy of the GNU " +
        "General Public License\r\n    along with this program; if not, write to the Free S" +
        "oftware\r\n    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-130" +
        "7  USA\r\n\r\n\r\nAlso add information on how to contact you by electronic and paper m" +
        "ail.\r\n\r\nIf the program is interactive, make it output a short notice like this\r\n" +
        "when it starts in an interactive mode:\r\n\r\n    Gnomovision version 69, Copyright " +
        "(C) year name of author\r\n    Gnomovision comes with ABSOLUTELY NO WARRANTY; for " +
        "details type `show w\'.\r\n    This is free software, and you are welcome to redist" +
        "ribute it\r\n    under certain conditions; type `show c\' for details.\r\n\r\nThe hypot" +
        "hetical commands `show w\' and `show c\' should show the appropriate\r\nparts of the" +
        " General Public License.  Of course, the commands you use may\r\nbe called somethi" +
        "ng other than `show w\' and `show c\'; they could even be\r\nmouse-clicks or menu it" +
        "ems--whatever suits your program.\r\n\r\nYou should also get your employer (if you w" +
        "ork as a programmer) or your\r\nschool, if any, to sign a \"copyright disclaimer\" f" +
        "or the program, if\r\nnecessary.  Here is a sample; alter the names:\r\n\r\n  Yoyodyne" +
        ", Inc., hereby disclaims all copyright interest in the program\r\n  `Gnomovision\' " +
        "(which makes passes at compilers) written by James Hacker.\r\n\r\n  <signature of Ty" +
        " Coon>, 1 April 1989\r\n  Ty Coon, President of Vice\r\n\r\nThis General Public Licens" +
        "e does not permit incorporating your program into\r\nproprietary programs.  If you" +
        "r program is a subroutine library, you may\r\nconsider it more useful to permit li" +
        "nking proprietary applications with the\r\nlibrary.  If this is what you want to d" +
        "o, use the GNU Library General\r\nPublic License instead of this License.";
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.Location = new System.Drawing.Point(312, 16);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(192, 16);
      this.label3.TabIndex = 4;
      this.label3.Text = "(C) 2004-2005 Dr. Dirk Lellinger";
      // 
      // m_LinkLabel
      // 
      this.m_LinkLabel.Location = new System.Drawing.Point(16, 80);
      this.m_LinkLabel.Name = "m_LinkLabel";
      this.m_LinkLabel.Size = new System.Drawing.Size(488, 16);
      this.m_LinkLabel.TabIndex = 5;
      this.m_LinkLabel.TabStop = true;
      this.m_LinkLabel.Text = "You can obtain the latest version of Syncoco from http://sourceforge.net/projects" +
        "/syncoco";
      this.m_LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_LinkLabel_LinkClicked);
      // 
      // AboutDialog
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(512, 458);
      this.Controls.Add(this.m_LinkLabel);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.m_btOK);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "AboutDialog";
      this.Text = "About Syncoco";
      this.ResumeLayout(false);

    }
    #endregion

    private void m_LinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
      // Determine which link was clicked within the LinkLabel.
      m_LinkLabel.Links[m_LinkLabel.Links.IndexOf(e.Link)].Visited = true;
      // Display the appropriate link based on the value of the LinkData property of the Link object.
      System.Diagnostics.Process.Start(e.Link.LinkData.ToString());

    }
  }
}
