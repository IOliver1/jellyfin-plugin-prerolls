# jellyfin-plugin-prerolls

Plays a random pre-roll video before **movies**, **TV episodes**, or **both** — your choice.  
Built for Adult Swim bumps. Targets **Jellyfin 10.11.x**. No Cinema Mode required.

---

## Build

You need [.NET 8 SDK](https://dotnet.microsoft.com/download).

```bat
build.bat
```

The script produces `jellyfin-plugin-prerolls-v1.0.0.zip` and prints the MD5 checksum.

---

## Publish to GitHub (so Jellyfin can install it)

1. Create a GitHub repo named `jellyfin-plugin-prerolls`
2. Push all files
3. Go to **Releases → Draft new release** → tag `v1.0.0`
4. Upload the `.zip` as a release asset
5. Copy the download URL (it looks like `https://github.com/YOU/jellyfin-plugin-prerolls/releases/download/v1.0.0/jellyfin-plugin-prerolls-v1.0.0.zip`)
6. Open `manifest.json` and replace:
   - `YOUR_USERNAME` → your GitHub username
   - `REPLACE_WITH_MD5_AFTER_BUILD` → the MD5 the build script printed
7. Push the updated `manifest.json`

---

## Install in Jellyfin

1. **Dashboard → Plugins → Repositories → `+`**
2. Paste:
   ```
   https://raw.githubusercontent.com/YOUR_USERNAME/jellyfin-plugin-prerolls/main/manifest.json
   ```
3. **Catalog → Prerolls → Install → Restart Jellyfin**

---

## Configure

**Dashboard → Plugins → Prerolls**

| Setting | Description |
|---|---|
| **Pre-roll Folder Path** | Full path to your bump folder, e.g. `D:\AdultSwimBumps` |
| **Play Pre-rolls Before** | `Movies only` / `TV Shows only` / `Both` |

A random `.mp4` (or `.mkv`, `.avi`, `.mov`, `.m4v`, `.webm`) from the folder is picked each time.

---

## How it works

Implements Jellyfin's `IIntroProvider` interface — the same hook used by the official intros plugin. Jellyfin calls it automatically at playback start. No Cinema Mode toggle needed, works on all clients that support server-side intros (web, Android, iOS, Jellyfin Media Player).
