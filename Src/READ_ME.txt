Scaffold

STH: Dette var min erfaring med brugen af Scaffold f�rste gang.

1) Blev n�dt til at kopiere Templates folderen ud i c:\Templates
2) create-project opretter et scaffold project
3) Filen c:\Templates\NewProject\ProjectName\ProjectName.Web\Common\Logging\ElmahHandleErrorAttribute.cs bliver ikke kopieret over fra templates og skal have sat namespace
4) G� ind i det oprettede scaffold projekt
5) Opret en entity
6) Nu kan der laves create-crud p� den oprettede entity
7) Der skal laves en database - man kan hurtigt �ndre NHibernateConfigurations i Core til at bruge SQLite (se testprojekt for config og s�rg for at SQLite ADO driver tilf�jes) Det kr�ver at det laves som en fil

8) Opgradering af NuGet pakker resulterer i at test fejler
8.1) I webconfig er der en fejl hvor ELMAH s�tter <security allowRemoteAccess="" /> to gange.

9) ADVARSEL! setup.bat overskriver din eksisterende path! For at kunne k�re de medf�lgende bat-filer er det n�dvendigt at k�re setup.bat f�rst - den pakker Ruby ud og s�tter din sti op.
10) Ved deploy er det n�dvendigt at konvertere folderen med de publiserede filer i til en applikation