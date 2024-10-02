using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public static class EmojiUtil
    {
        public static Texture FromString(string emoji)
        {
            return FromUnicode(ToUnicodes(emoji));
        }
        public static Texture FromUnicode(params uint[] unicodes)
        {
            if (unicodes.Length <= 0)
            {
                return null;
            }
            var emoji = Emoji();
            if (emoji is null)
            {
                return null;
            }
            string name = string.Join("-", Array.ConvertAll(unicodes, v => v.ToString("x4")));
            Debug.Log(name);
            return FromName(name);
        }
        public static Texture FromName(string name)
        {
            if (!_emojiTextures.TryGetValue(name, out var texture))
            {
                texture = CreateTexture(name);
                _emojiTextures.Add(name, texture);
            }
            return texture;
        }
        public static string GetAlias(uint unicode)
        {
            var emoji = Emoji();
            if (emoji is null)
            {
                return string.Empty;
            }
            var index = emoji.GetSpriteIndexFromUnicode(unicode);
            if (index < 0 || emoji.spriteGlyphTable.Count <= 0)
            {
                return string.Empty;
            }
            var c = emoji.spriteCharacterTable[index];
            return c.name;
        }
        public static uint[] GetUnicodes()
        {
            var emoji = Emoji();
            if (emoji is null)
            {
                return new uint[0];
            }
            return emoji.spriteCharacterTable.Select(c => c.unicode).ToArray();
        }
        public static string[] GetNames()
        {
            var emoji = Emoji();
            if (emoji is null)
            {
                return new string[0];
            }
            return emoji.spriteCharacterTable.Select(c => c.name).ToArray();
        }
        static Texture2D CreateTexture(string name)
        {
            var emoji = Emoji();
            if (emoji is null)
            {
                return null;
            }
            Texture2D originalTexture = emoji.spriteSheet as Texture2D;
            if (originalTexture is null)
            {
                return null;
            }
            var index = emoji.GetSpriteIndexFromName(name);
            if (index < 0)
            {
                return null;
            }
            TMP_SpriteGlyph glyph = emoji.spriteGlyphTable[index];

            // テクスチャの情報を使用して、新しいテクスチャを生成
            var rect = glyph.glyphRect;
            Texture2D newTexture = new Texture2D(rect.width, rect.height);
            Color[] pixels = originalTexture.GetPixels(rect.x, rect.y, rect.width, rect.height);
            newTexture.SetPixels(pixels);
            newTexture.Apply();

            return newTexture;
        }
        static uint[] ToUnicodes(string characters)
        {
            List<uint> list = new List<uint>(characters.Length);
            for (int i = 0; i < characters.Length; i++)
            {
                uint unicode = characters[i];
                if (i < characters.Length - 1 && char.IsHighSurrogate((char)unicode) && char.IsLowSurrogate(characters[i + 1]))
                {
                    unicode = (uint)char.ConvertToUtf32(characters[i], characters[i + 1]);
                    i++;
                }
                list.Add(unicode);
            }
            return list.ToArray();
        }
        static TMP_SpriteAsset Emoji()
        {
            return _emojiAsset ??= LoadEmojiAsset();
        }
        static TMPro.TMP_SpriteAsset LoadEmojiAsset()
        {
            return LoadPackageAsset<TMPro.TMP_SpriteAsset>("Resources/Emoji/EmojiData.asset");
        }
        static T LoadPackageAsset<T>(string path)
            where T : UnityEngine.Object
        {
            var ret = AssetDatabase.LoadAssetAtPath<T>("Packages/com.tyanmahou.mystic-launcher/" + path);
            if (ret is null)
            {
                ret = AssetDatabase.LoadAssetAtPath<T>("Assets/MysticLauncher/" + path);
            }
            return ret;
        }
        static TMP_SpriteAsset _emojiAsset;
        static Dictionary<string, Texture> _emojiTextures = new();
    }
}
