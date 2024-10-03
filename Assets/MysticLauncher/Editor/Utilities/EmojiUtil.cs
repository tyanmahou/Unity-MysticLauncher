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
        public static Texture FromRaw(string emoji)
        {
            return FromUnicode(ToUnicodes(emoji));
        }
        public static Texture FromUnicode(params uint[] unicodes)
        {
            if (unicodes.Length <= 0)
            {
                return null;
            }
            return FromUnicodeKey(ToUnicodeKey(unicodes));
        }
        public static Texture FromUnicodeKey(string unicodeKey)
        {
            if (!_emojiTextures.TryGetValue(unicodeKey, out var texture))
            {
                texture = CreateTexture(unicodeKey);
                _emojiTextures.Add(unicodeKey, texture);
            }
            return texture;
        }
        public static string GetShortName(string unicodeKey)
        {
            var emoji = EmojiData();
            if (emoji is null)
            {
                return string.Empty;
            }
            return emoji.GetShortName(unicodeKey);
        }
        public static string[] GetUnicodeKeys()
        {
            var emoji = EmojiSprite();
            if (emoji is null)
            {
                return new string[0];
            }
            return emoji.spriteCharacterTable.Select(c => c.name).ToArray();
        }
        public static string[] GetUnicodeKeys(string search)
        {
            var keys = GetUnicodeKeys();
            var emoji = EmojiData();
            if (emoji is null)
            {
                return keys;
            }
            return keys.Where(k => emoji.IsSearched(k, search)).ToArray();
        }
        public static string GetRaw(string unicodeKey)
        {
            string[] codePoints = unicodeKey.Split('-');

            string emoji = string.Empty;
            foreach (string codePoint in codePoints)
            {
                int unicode = int.Parse(codePoint, System.Globalization.NumberStyles.HexNumber);
                emoji += char.ConvertFromUtf32(unicode);
            }

            return emoji;
        }
        static Texture2D CreateTexture(string unicodeKey)
        {
            var emoji = EmojiSprite();
            if (emoji is null)
            {
                return null;
            }
            Texture2D originalTexture = emoji.spriteSheet as Texture2D;
            if (originalTexture is null)
            {
                return null;
            }
            var index = emoji.GetSpriteIndexFromName(unicodeKey);
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
        static string ToUnicodeKey(params uint[] unicodes)
        {
            return string.Join("-", Array.ConvertAll(unicodes, v => v.ToString("x4")));
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
        static EmojiDataList EmojiData()
        {
            return _emojiData ??= LoadPackageAsset<EmojiDataList>("Resources/Emoji/EmojiData.asset");
        }
        static TMP_SpriteAsset EmojiSprite()
        {
            return _emojiSprite ??= LoadPackageAsset<TMPro.TMP_SpriteAsset>("Resources/Emoji/EmojiSprite.asset");
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
        static EmojiDataList _emojiData;
        static TMP_SpriteAsset _emojiSprite;
        static Dictionary<string, Texture> _emojiTextures = new();
    }
}
