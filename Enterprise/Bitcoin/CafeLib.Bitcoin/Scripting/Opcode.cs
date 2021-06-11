﻿#region Copyright
// Copyright (c) 2020 TonesNotes
// Distributed under the Open BSV software license, see the accompanying file LICENSE.
#endregion
// ReSharper disable InconsistentNaming
namespace CafeLib.Bitcoin.Scripting
{
    /// <summary>
    /// Script opcodes 
    /// </summary>
    public enum Opcode : byte
    {
        // push value
        OP_0 = 0x00,
        OP_FALSE = OP_0,
        // The following opcodes specify how many of the following bytes to push.
        OP_PUSH1 = 1,
        OP_PUSH2 = 2,
        OP_PUSH3 = 3,
        OP_PUSH4 = 4,
        OP_PUSH5 = 5,
        OP_PUSH6 = 6,
        OP_PUSH7 = 7,
        OP_PUSH8 = 8,
        OP_PUSH9 = 9,
        OP_PUSH10 = 10,
        OP_PUSH11 = 11,
        OP_PUSH12 = 12,
        OP_PUSH13 = 13,
        OP_PUSH14 = 14,
        OP_PUSH15 = 15,
        OP_PUSH16 = 16,
        OP_PUSH17 = 17,
        OP_PUSH18 = 18,
        OP_PUSH19 = 19,
        OP_PUSH20 = 20,
        OP_PUSH21 = 21,
        OP_PUSH22 = 22,
        OP_PUSH23 = 23,
        OP_PUSH24 = 24,
        OP_PUSH25 = 25,
        OP_PUSH26 = 26,
        OP_PUSH27 = 27,
        OP_PUSH28 = 28,
        OP_PUSH29 = 29,
        OP_PUSH30 = 30,
        OP_PUSH31 = 31,
        OP_PUSH32 = 32,
        OP_PUSH33 = 33,
        OP_PUSH34 = 34,
        OP_PUSH35 = 35,
        OP_PUSH36 = 36,
        OP_PUSH37 = 37,
        OP_PUSH38 = 38,
        OP_PUSH39 = 39,
        OP_PUSH40 = 40,
        OP_PUSH41 = 41,
        OP_PUSH42 = 42,
        OP_PUSH43 = 43,
        OP_PUSH44 = 44,
        OP_PUSH45 = 45,
        OP_PUSH46 = 46,
        OP_PUSH47 = 47,
        OP_PUSH48 = 48,
        OP_PUSH49 = 49,
        OP_PUSH50 = 50,
        OP_PUSH51 = 51,
        OP_PUSH52 = 52,
        OP_PUSH53 = 53,
        OP_PUSH54 = 54,
        OP_PUSH55 = 55,
        OP_PUSH56 = 56,
        OP_PUSH57 = 57,
        OP_PUSH58 = 58,
        OP_PUSH59 = 59,
        OP_PUSH60 = 60,
        OP_PUSH61 = 61,
        OP_PUSH62 = 62,
        OP_PUSH63 = 63,
        OP_PUSH64 = 64,
        OP_PUSH65 = 65,
        OP_PUSH66 = 66,
        OP_PUSH67 = 67,
        OP_PUSH68 = 68,
        OP_PUSH69 = 69,
        OP_PUSH70 = 70,
        OP_PUSH71 = 71,
        OP_PUSH72 = 72,
        OP_PUSH73 = 73,
        OP_PUSH74 = 74,
        OP_PUSH75 = 75,
        // The byte following this opcode specifies how many bytes after it to push.
        OP_PUSHDATA1 = 0x4c,
        // The two bytes following this opcode specify how many bytes after it to push.
        OP_PUSHDATA2 = 0x4d,
        // The four bytes following this opcode specify how many bytes after it to push.
        OP_PUSHDATA4 = 0x4e,
        OP_1NEGATE = 0x4f,
        OP_RESERVED = 0x50,
        // The following opcodes push the specified value.
        OP_1 = 0x51,
        OP_TRUE = OP_1,
        OP_2 = 0x52,
        OP_3 = 0x53,
        OP_4 = 0x54,
        OP_5 = 0x55,
        OP_6 = 0x56,
        OP_7 = 0x57,
        OP_8 = 0x58,
        OP_9 = 0x59,
        OP_10 = 0x5a,
        OP_11 = 0x5b,
        OP_12 = 0x5c,
        OP_13 = 0x5d,
        OP_14 = 0x5e,
        OP_15 = 0x5f,
        OP_16 = 0x60,

        // control
        OP_NOP = 0x61,
        OP_VER = 0x62,
        OP_IF = 0x63,
        OP_NOTIF = 0x64,
        OP_VERIF = 0x65,
        OP_VERNOTIF = 0x66,
        OP_ELSE = 0x67,
        OP_ENDIF = 0x68,
        OP_VERIFY = 0x69,
        OP_RETURN = 0x6a,

        // stack ops
        OP_TOALTSTACK = 0x6b,
        OP_FROMALTSTACK = 0x6c,
        OP_2DROP = 0x6d,
        OP_2DUP = 0x6e,
        OP_3DUP = 0x6f,
        OP_2OVER = 0x70,
        OP_2ROT = 0x71,
        OP_2SWAP = 0x72,
        OP_IFDUP = 0x73,
        OP_DEPTH = 0x74,
        OP_DROP = 0x75,
        OP_DUP = 0x76,
        OP_NIP = 0x77,
        OP_OVER = 0x78,
        OP_PICK = 0x79,
        OP_ROLL = 0x7a,
        OP_ROT = 0x7b,
        OP_SWAP = 0x7c,
        OP_TUCK = 0x7d,

        // splice ops
        OP_CAT = 0x7e,
        OP_SPLIT = 0x7f,   // after monolith upgrade (May 2018)
        OP_NUM2BIN = 0x80, // after monolith upgrade (May 2018)
        OP_BIN2NUM = 0x81, // after monolith upgrade (May 2018)
        OP_SIZE = 0x82,

        // bit logic
        OP_INVERT = 0x83,
        OP_AND = 0x84,
        OP_OR = 0x85,
        OP_XOR = 0x86,
        OP_EQUAL = 0x87,
        OP_EQUALVERIFY = 0x88,
        OP_RESERVED1 = 0x89,
        OP_RESERVED2 = 0x8a,

        // numeric
        OP_1ADD = 0x8b,
        OP_1SUB = 0x8c,
        OP_2MUL = 0x8d,
        OP_2DIV = 0x8e,
        OP_NEGATE = 0x8f,
        OP_ABS = 0x90,
        OP_NOT = 0x91,
        OP_0NOTEQUAL = 0x92,

        OP_ADD = 0x93,
        OP_SUB = 0x94,
        OP_MUL = 0x95,
        OP_DIV = 0x96,
        OP_MOD = 0x97,
        OP_LSHIFT = 0x98,
        OP_RSHIFT = 0x99,

        OP_BOOLAND = 0x9a,
        OP_BOOLOR = 0x9b,
        OP_NUMEQUAL = 0x9c,
        OP_NUMEQUALVERIFY = 0x9d,
        OP_NUMNOTEQUAL = 0x9e,
        OP_LESSTHAN = 0x9f,
        OP_GREATERTHAN = 0xa0,
        OP_LESSTHANOREQUAL = 0xa1,
        OP_GREATERTHANOREQUAL = 0xa2,
        OP_MIN = 0xa3,
        OP_MAX = 0xa4,

        OP_WITHIN = 0xa5,

        // crypto
        OP_RIPEMD160 = 0xa6,
        OP_SHA1 = 0xa7,
        OP_SHA256 = 0xa8,
        OP_HASH160 = 0xa9,
        OP_HASH256 = 0xaa,
        OP_CODESEPARATOR = 0xab,
        OP_CHECKSIG = 0xac,
        OP_CHECKSIGVERIFY = 0xad,
        OP_CHECKMULTISIG = 0xae,
        OP_CHECKMULTISIGVERIFY = 0xaf,

        // expansion
        OP_NOP1 = 0xb0,
        OP_CHECKLOCKTIMEVERIFY = 0xb1,
        OP_NOP2 = OP_CHECKLOCKTIMEVERIFY,
        OP_CHECKSEQUENCEVERIFY = 0xb2,
        OP_NOP3 = OP_CHECKSEQUENCEVERIFY,
        OP_NOP4 = 0xb3,
        OP_NOP5 = 0xb4,
        OP_NOP6 = 0xb5,
        OP_NOP7 = 0xb6,
        OP_NOP8 = 0xb7,
        OP_NOP9 = 0xb8,
        OP_NOP10 = 0xb9,

        // The first op_code value after all defined opcodes
        FIRST_UNDEFINED_OP_VALUE,

        // template matching params
        OP_SMALLINTEGER = 0xfa,
        OP_PUBKEYS = 0xfb,
        OP_PUBKEYHASH = 0xfd,
        OP_PUBKEY = 0xfe,

        OP_INVALIDOPCODE = 0xff,
    };
}