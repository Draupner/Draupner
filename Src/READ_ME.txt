Scaffold

STH: Dette var min erfaring med brugen af Scaffold første gang.

1) Blev nødt til at kopiere Templates folderen ud i c:\Templates
2) create-project opretter et scaffold project
3) Filen c:\Templates\NewProject\ProjectName\ProjectName.Web\Common\Logging\ElmahHandleErrorAttribute.cs bliver ikke kopieret over fra templates og skal have sat namespace
4) Gå ind i det oprettede scaffold projekt
5) Opret en entity
6) Nu kan der laves create-crud på den oprettede entity
7) Der skal laves en database - man kan hurtigt ændre NHibernateConfigurations i Core til at bruge SQLite (se testprojekt for config og sørg for at SQLite ADO driver tilføjes) Det kræver at det laves som en fil

8) Opgradering af NuGet pakker resulterer i at test fejler
8.1) I webconfig er der en fejl hvor ELMAH sætter <security allowRemoteAccess="" /> to gange.

9) ADVARSEL! setup.bat overskriver din eksisterende path! For at kunne køre de medfølgende bat-filer er det nødvendigt at køre setup.bat først - den pakker Ruby ud og sætter din sti op.
10) Ved deploy er det nødvendigt at konvertere folderen med de publiserede filer i til en applikation