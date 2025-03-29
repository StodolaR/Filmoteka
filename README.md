# Filmoteka
WPF aplikace, .Net8.0, pokus o MVVM, databáze SQLite pomocí EF Core. Ke spuštění je potřeba .Net8.0
## Screenshots
![Úvod](https://github.com/StodolaR/Filmoteka/blob/master/Screenshots/Filmoteka.jpg)
![Přihlášení](https://github.com/StodolaR/Filmoteka/blob/master/Screenshots/Prihlaseni.jpg)
![Uživatelé](https://github.com/StodolaR/Filmoteka/blob/master/Screenshots/Uzivatele.jpg)
![Žebříček](https://github.com/StodolaR/Filmoteka/blob/master/Screenshots/Zebricek.jpg)
![Žánry](https://github.com/StodolaR/Filmoteka/blob/master/Screenshots/Zanry.jpg)
![Detail](https://github.com/StodolaR/Filmoteka/blob/master/Screenshots/Detail.jpg)
## Popis funkcí Filmotéky

Mezi okny aplikace se přepíná pomocí záložek v horní části.
#### Žebříček 
- zobrazuje vložené filmy seřazené podle průměrného hodnocení uživatelů
- kliknutí na film zobrazí jeho detail
#### Dle žánru 
- zobrazuje vložené filmy rozdělené dle žánru a dále seřazené dle hodnocení uživatelů
- kliknutí na film zobrazí jeho detail
#### Uživatelé 
- zobrazí registrované uživatele seřazené dle abecedy
- Kliknutí na uživatele zobrazí jím přidaná hodnocení filmu
- Kliknutím na ikonu pod hodnocením se případně zobrazí připojená recenze
#### Přihlášení 
- Zde je napravo možnost zaregistrovat nového uživatele
- aplikace hlídá správnost vyplnění kolonek (aby jméno i heslo bylo alespoň 3 znaky dlouhé,
             heslo bylo správně potvrzeno a aby nebylo použito jméno již existujícího uživatele)
             (hesla uživatelů v ukázkové databázi jsou jejich jména s přidanými číslicemi 123 - např: Pepa123 )
- po registraci je uživatel automaticky přihlášen
- nalevo lze případně odhlásit přihlášeného uživatele a přihlásit jiného
- přihlášený uživatel má možnost v záložce "Žebříček" přidávat další filmy
             (je hlídáno, aby kombinace jména a roku přidávaného filmu byla jedinečná a aby rok byl zadán čtyřmi číslicemi 
             v rozmezí 1900 - současný rok, hodnocení se nastavuje kliknutím na hvězdičku, kliknutí na 0 = 0%)
- dále může v detailu filmu (po zvolení filmu z žebříčku) přidat, či změnit své původní hodnocení, případně i recenzi filmu
- uživatelé nemohou již vytvořený film měnit, změny lze provést tímto "tajným" postupem:
  - v "Přihlášení uživatele" do kolonky "Uživatelské jméno" zadat slovo Movie,
               do hesla jedno ze slov: Name, Genre, Description, Picture či Delete.
  - v detailu editovaného filmu lze poté měnit vlastnost filmu podle toho, jaké heslo bylo zadáno:
    - při "Name" lze měnit jméno filmu či rok výroby (kombinace jména a roku tvoří jedinečný identifikátor)
    - při "Genre" lze přenastavit žánr
    - při "Description" lze změnit popis
    - při "Picture" změnit či přidat obrázek filmy (při vytváření filmu není obrázek povinný)
    - při "Delete" lze film smazat (aplikace poté skočí na úvodní obrazovku)
#### Vyhledávání 
- lze zadat část slova, celé slovo či více slov, u více slov se aplikace nejprve pokusí najít ve jménech filmů
               slovní spojení a pokud nenalezne, hledá ve jménech jednotlivá slova... pokud nenalezne, vypíše "Film nenalezen".
               Výběr ze seznamu výsledků pak otevře detail daného filmu.

Pokud z pracovniho adresáře (Filmoteka\bin\Debug\net8.0-windows) odstraníme soubor "movies.db a složku "Posters", 
získáme čistou databázi pro naplnění vlastními filmy a hodnoceními.
Pokud do pracovního adresáře zkopírujeme obsah složky "Zaloha-ukazka", získáme zpět ukázkovou databázi.
