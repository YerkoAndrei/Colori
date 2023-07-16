using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.GenericParameter | AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class OcultarAttribute : PropertyAttribute
{
    public enum TipoOcultar
    {
        ocultar,
        desaparecerPorBool,
        desaparecerPorInt
    }

    //TRUE  = Desabilitar
    //FALSE = Ocultar
    public bool deshabilitar;

    public TipoOcultar tipoOcultar;
    public string variableCondicional;
    public int valorInt;

    public OcultarAttribute()
    {
        this.tipoOcultar = TipoOcultar.ocultar;
        this.deshabilitar = false;
    }

    public OcultarAttribute(string variableCondicional)
    {
        this.tipoOcultar = TipoOcultar.desaparecerPorBool;
        this.variableCondicional = variableCondicional;
        this.deshabilitar = true;
    }

    public OcultarAttribute(string variableCondicional, int intCondition)
    {
        this.tipoOcultar = TipoOcultar.desaparecerPorInt;
        this.variableCondicional = variableCondicional;
        this.deshabilitar = true;
        this.valorInt = intCondition;
    }
}
