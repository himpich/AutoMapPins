﻿using System.Collections.Generic;
using System.Linq;
using AutoMapPins.Data;
using AutoMapPins.Icons;
using AutoMapPins.Model;
using HarmonyLib;
using JetBrains.Annotations;

namespace AutoMapPins.Patches
{
    [HarmonyPatch(typeof(Minimap), nameof(Minimap.LoadMapData))]
    class MinimapPatch
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref Minimap __instance)
        {
            List<Minimap.PinData> pins = __instance.m_pins;
            AutoMapPinsPlugin.Log.LogInfo($"Loaded map with {pins.Count} existing pins");
            foreach (var pin in pins)
            {
                PinConfig? config = Registry.ConfiguredPins
                    .Select(config => config.Value)
                    .FirstOrDefault(config => config.Name == pin.m_name);
                if (config != null) pin.m_icon = Assets.GetIcon(config.IconName);
            }
        }
    }
}