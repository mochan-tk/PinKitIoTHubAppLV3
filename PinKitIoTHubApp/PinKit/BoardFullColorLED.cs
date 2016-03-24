using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace PinKit
{
    public class BoardFullColorLED
    {
               // �e LED �̃s���ԍ�
        private const Cpu.Pin pinRed = (Cpu.Pin)0x6d;       // ��
        private const Cpu.Pin pinGreen = (Cpu.Pin)0x6e;     // ��
        private const Cpu.Pin pinBlue = (Cpu.Pin)0x6f;      // ��

        // �e LED ���ڑ������|�[�g
        private OutputPort portRed;     // ��
        private OutputPort portGreen;   // ��
        private OutputPort portBlue;    // ��


        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        public BoardFullColorLED()
        {
            // �e LED �� InputPort �C���X�^���X
            portRed = new OutputPort(pinRed, false);
            portGreen = new OutputPort(pinGreen, false);
            portBlue = new OutputPort(pinBlue, false);
        }

        /// <summary>
        /// �w��̐F�� LED ��_���A��������
        /// </summary>
        /// <param name="redOn">true �Ȃ�ΐԂ�_��</param>
        /// <param name="greenOn">true �Ȃ�Η΂�_��</param>
        /// <param name="blueOn">true �Ȃ�ΐ�_��</param>
        public void SetRgb(bool redOn, bool greenOn, bool blueOn)
        {
            portRed.Write(redOn);
            portGreen.Write(greenOn);
            portBlue.Write(blueOn);
        }

        /// <summary>
        /// �F���w��� LED ��_������
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Colors color)
        {
            int redFlag = (int)color & (int)Colors.Red;
            int greenFlag = (int)color & (int)Colors.Green;
            int blueFlag = (int)color & (int)Colors.Blue;
            portRed.Write(redFlag != 0);
            portGreen.Write(greenFlag != 0);
            portBlue.Write(blueFlag != 0);
        }

        public enum Colors
        {
            Black = 0,
            Red = 1,
            Green = 2,
            Yellow = 3,
            Blue = 4,
            Magenta = 5,
            Cyan = 6,
            White = 7
        }
    }
}
