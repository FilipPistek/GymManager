# GymManager

## Autor
**Jméno:** Filip Pištěk

**Předmět:** Programové vybavení

**Rok:** 2025

## Popis projektu
Gym Manager je konzolová aplikace vyvinutá v jazyce C# (.NET), která slouží ke správě agendy fitness centra. Projekt je realizován v souladu se zadáním D1 a využívá návrhový vzor Repository Pattern, který striktně odděluje databázovou vrstvu od aplikační logiky.
Aplikace pracuje s relační databází MS SQL. Databázový model obsahuje 5 tabulek propojených vazbami (včetně M:N) a využívá pohledy (Views).

## Struktura repozitáře

* `Entities/` Třídy reprezentující tabulky v DB.
* `Repositories/` Vrstva pro přímou komunikaci s SQL..
* `Services/` Pomocné služby pro logiku, která nepatří přímo do repozitáře (např. parsování CSV souborů, formátování reportů).
* `TestingData/` Zde se nachází `.csv` soubory s daty pro import funkce.
* `Program.cs` Uživatelské rozhraní (Konzole) a hlavní smyčka programu.

## Návod na instalaci a spuštění
Aby aplikace fungovala, je nutné provést následující kroky:

1.  **Stažení:**
    * Přejděte do sekce **[Releases](../../releases)** v tomto repozitáři.
    * Stáhněte a rozbalte `.zip` archiv.

2.  **Vytvoření Databáze**
    * Otevřete **MS SQL**.
    * Vytvořte novou databázi (např. `GymDB`).
    * V kořenu projektu najděte **`database_setup.sql`**.
    * Spusťte sql ve vaší vytvořené databázi
    * Vytvoří se tabulky `Clients`, `Trainers`, `Lessons`, `Bookings`, `Logs` a `Views`.

3.  **Spuštění:**
    * Upravte si v App.config sekci `<connectionStrings>` upravte hodnoty, které jsou napsaná VELKÝM PÍSMEM na vlastní.
    * Spusťte aplikaci `GymManager.exe`.
