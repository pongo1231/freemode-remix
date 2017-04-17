﻿using CitizenFX.Core;
using CitizenFX.Core.UI;
using Freeroam.Utils;
using NativeUI;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Freeroam.Holders
{
    public class Money : BaseScript
    {
        public const string PNAME_MONEY = "_PLAYER_MONEY";

        private int moneyChangeTextAmount;
        private int moneyChangeTextShowProgress;

        public static int MONEY { get; private set; }

        public Money()
        {
            MONEY = Storage.GetInt(Storage.MONEY);

            EventHandlers[Events.MONEY_ADD] += new Action<int>(AddMoney);
            EventHandlers[Events.MONEY_REMOVE] += new Action<int>(RemoveMoney);

            Tick += OnTick;
        }

        private void AddMoney(int amount)
        {
            SetMoney(MONEY + amount);
        }

        private void RemoveMoney(int amount)
        {
            SetMoney(MONEY - amount);
        }

        private void SetMoney(int newMoney)
        {
            moneyChangeTextAmount = newMoney - MONEY;
            moneyChangeTextShowProgress = 300;

            MONEY = newMoney < 0 ? 0 : newMoney;
            Storage.SetInt(Storage.MONEY, MONEY);
        }

        public static int GetPlayerMoney(Player player)
        {
            Ped playerPed = player.Character;
            if (playerPed == null || !EntityDecoration.ExistOn(playerPed, PNAME_MONEY)) return 0;
            else return EntityDecoration.Get<int>(playerPed, PNAME_MONEY);
        }

        private async Task OnTick()
        {
            DrawMoneyText();

            if (moneyChangeTextShowProgress > 0)
            {
                DrawMoneyChangeText();
                moneyChangeTextShowProgress--;
            }

            await Task.FromResult(0);
        }

        private void DrawMoneyText()
        {
            UIResText moneyText = new UIResText($"{MONEY} $", new PointF(320f, 1024f), 0.7f, Color.FromArgb(255, 0, 153, 0),
                Font.ChaletComprimeCologne, UIResText.Alignment.Left);
            moneyText.DropShadow = true;
            moneyText.Draw();
        }

        private void DrawMoneyChangeText()
        {
            UIResText moneyChangeText = new UIResText("", new PointF(320f, 995f), 0.5f, Color.Empty,
                Font.ChaletComprimeCologne, UIResText.Alignment.Left);

            if (moneyChangeTextAmount > 0)
            {
                moneyChangeText.Caption = $"+ {moneyChangeTextAmount}$";
                moneyChangeText.Color = Color.FromArgb(255, 0, 153, 0);
            }
            else
            {
                moneyChangeText.Caption = $"- {moneyChangeTextAmount * -1}$";
                moneyChangeText.Color = Color.FromArgb(255, 153, 0, 0);
            }

            moneyChangeText.Draw();
        }
    }
}
