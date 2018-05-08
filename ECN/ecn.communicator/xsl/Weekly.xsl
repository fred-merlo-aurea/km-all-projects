<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="no" encoding="utf-8" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <table width="100%" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="schd_blu">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr align="left" valign="top">             
              <td height="24" align="center" valign="middle" class="txtschd_s11" style="padding-top:5px; padding-bottom:5px; border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;border-left-color: #CDCDCD; border-left-style: solid; border-width: thin 1px;border-top-color: #CDCDCD; border-top-style: solid; border-width: thin 1px;">
                <table width="100%" border="0" cellspacing="4" cellpadding="0">
                  <tr align="center" valign="middle">
                    <td align="left">
                      <xsl:element name="a">
                        <xsl:attribute name="href">
                          <xsl:value-of select="//PrevWeekLink" />
                        </xsl:attribute>
                        <xsl:attribute name="title">
                          Go to Previous Week
                        </xsl:attribute>
                        <img src="../../images/calendar-previous.png" border="0" />
                      </xsl:element>
                    </td>
                    <td>
                      <b style="font-size:14px; font-weight:bold; font-family:arial;">     
                        <xsl:value-of select="//BlastWeek" />
                      </b>
                    </td>
                    <td align="right">
                      <xsl:element name="a">
                        <xsl:attribute name="href">
                          <xsl:value-of select="//NextWeekLink" />
                        </xsl:attribute>
                        <xsl:attribute name="title">
                          Go to Next Week
                        </xsl:attribute>
                        <img src="../../images/calendar-next.png" border="0" />
                      </xsl:element>
                    </td>
                  </tr>
                </table>
              </td>             
            </tr>
          </table>
        </td>
      </tr>     
      <tr>
        <td align="left" valign="top" class="scbr_blu">
          <table width="100%" border="0" cellspacing="0" cellpadding="4" class="Border_0px" style="border-color: #CDCDCD; border-style: solid; border-width: thin 1px;">
            <tr align="left" valign="middle">
              <td width="50%" class="blugrid_col1 txtBlkHL" style="border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">              
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="//Head1/@link" />
                  </xsl:attribute>
                  <xsl:value-of select="//Head1" />                
                </xsl:element>
              </td>
              <td width="50%" class="blugrid_col1 blugrid_div txtBlkHL" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">             
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="//Head4/@link" />
                  </xsl:attribute>
                  <xsl:value-of select="//Head4" />
                </xsl:element>
              </td>
            </tr>
            <tr valign="top">
              <td height="50" class="blugrid_col2" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px">
                &#160;
                <xsl:for-each select="//Day1">
                  <xsl:for-each select="a">
                    <xsl:attribute name="class">LinkBlkUL</xsl:attribute>
                    <xsl:element name="a">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select="@style" />
                      </xsl:attribute>
                      <xsl:attribute name="title">
                        <xsl:value-of select="@title" />
                      </xsl:attribute>
                      <xsl:value-of select="." />
                    </xsl:element>
                    <br/>
                    &#160;
                  </xsl:for-each>
                </xsl:for-each>
              </td>
              <td class="blugrid_col2 blugrid_div" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                &#160;
                <xsl:for-each select="//Day4">
                  <xsl:for-each select="a">
                    <xsl:attribute name="class">LinkBlkUL</xsl:attribute>
                    <xsl:element name="a">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select="@style" />
                      </xsl:attribute>
                      <xsl:attribute name="title">
                        <xsl:value-of select="@title" />
                      </xsl:attribute>
                      <xsl:value-of select="." />                     
                    </xsl:element>
                    <br />
                    &#160;
                  </xsl:for-each>
                </xsl:for-each>
              </td>
            </tr>
            <tr align="left" valign="middle">
              <td class="blugrid_col1 txtBlkHL" style="border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="//Head2/@link" />
                  </xsl:attribute>
                  <xsl:value-of select="//Head2" />
                </xsl:element>
              </td>
              <td class="blugrid_col1 blugrid_div txtBlkHL" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="//Head5/@link" />
                  </xsl:attribute>
                  <xsl:value-of select="//Head5" />
                </xsl:element>
              </td>
            </tr>
            <tr valign="top">
              <td height="50" class="blugrid_col2" style="border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px; border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">               
                &#160;
                <xsl:for-each select="//Day2">
                  <xsl:for-each select="a">
                    <xsl:attribute name="class">LinkBlkUL</xsl:attribute>
                    <xsl:element name="a">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select="@style" />
                      </xsl:attribute>
                      <xsl:attribute name="title">
                        <xsl:value-of select="@title" />
                      </xsl:attribute>
                      <xsl:value-of select="." />                   
                    </xsl:element>
                    <br />
                    &#160;
                  </xsl:for-each>
                </xsl:for-each>
              </td>
              <td class="blugrid_col2 blugrid_div" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                &#160;
                <xsl:for-each select="//Day5">
                  <xsl:for-each select="a">
                    <xsl:attribute name="class">LinkBlkUL</xsl:attribute>
                    <xsl:element name="a">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select="@style" />
                      </xsl:attribute>
                      <xsl:attribute name="title">
                        <xsl:value-of select="@title" />
                      </xsl:attribute>
                      <xsl:value-of select="." />                    
                    </xsl:element>
                    <br />
                    &#160;
                  </xsl:for-each>
                </xsl:for-each>
              </td>
            </tr>
            <tr align="left" valign="middle">
              <td class="blugrid_col1 txtBlkHL" style="border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="//Head3/@link" />
                  </xsl:attribute>
                  <xsl:value-of select="//Head3" />
                </xsl:element>
              </td>
              <td class="blugrid_col1 blugrid_div txtBlkHL" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="//Head6/@link" />
                  </xsl:attribute>
                  <xsl:value-of select="//Head6" />
                </xsl:element>
              </td>
            </tr>
            <tr valign="top">
              <td height="50" class="blugrid_col2" style="border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                &#160;
                <xsl:for-each select="//Day3">
                  <xsl:for-each select="a">
                    <xsl:attribute name="class">LinkBlkUL</xsl:attribute>
                    <xsl:element name="a">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select="@style" />
                      </xsl:attribute>
                      <xsl:attribute name="title">
                        <xsl:value-of select="@title" />
                      </xsl:attribute>
                      <xsl:value-of select="." />                  
                    </xsl:element>
                    <br />
                    &#160;
                  </xsl:for-each>
                </xsl:for-each>
              </td>
              <td class="blugrid_col2 blugrid_div" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                &#160;
                <xsl:for-each select="//Day6">
                  <xsl:for-each select="a">
                    <xsl:attribute name="class">LinkBlkUL</xsl:attribute>
                    <xsl:element name="a">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select="@style" />
                      </xsl:attribute>
                      <xsl:attribute name="title">
                        <xsl:value-of select="@title" />
                      </xsl:attribute>
                      <xsl:value-of select="." />                    
                    </xsl:element>
                    <br />
                    &#160;
                  </xsl:for-each>
                </xsl:for-each>
              </td>
            </tr>
            <tr align="left" valign="middle">
              <td class="blugrid_col1 txtRedHL" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;">
                <xsl:element name="a">
                  <xsl:attribute name="href">
                    <xsl:value-of select="//Head7/@link" />    
                  </xsl:attribute>
                  <xsl:value-of select="//Head7" />
                </xsl:element>
              </td>
              <td class="blugrid_col1 txtBlkHL" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">&#160;</td>
            </tr>
            <tr valign="top">
              <td height="50" colspan="2" class="blugrid_col2">
                &#160;
                <xsl:for-each select="//Day7">
                  <xsl:for-each select="a">
                    <xsl:attribute name="class">LinkBlkUL</xsl:attribute>
                    <xsl:element name="a">
                      <xsl:attribute name="href">
                        <xsl:value-of select="@href" />
                      </xsl:attribute>
                      <xsl:attribute name="style">
                        <xsl:value-of select="@style" />
                      </xsl:attribute>
                      <xsl:attribute name="title">
                        <xsl:value-of select="@title" />
                      </xsl:attribute>
                      <xsl:value-of select="." />                     
                    </xsl:element>
                    <br />
                    &#160;
                  </xsl:for-each>
                </xsl:for-each>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>