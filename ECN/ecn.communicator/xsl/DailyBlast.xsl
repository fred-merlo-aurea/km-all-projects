<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="no" encoding="utf-8" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td align="left" valign="top" class="schd_blu">
          <table width="100%" border="0" cellspacing="0" cellpadding="0" style="border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;border-left-color: #CDCDCD; border-left-style: solid; border-width: thin 1px;border-top-color: #CDCDCD; border-top-style: solid; border-width: thin 1px;">
            <tr align="left" valign="top">
              <td height="24" align="center" valign="middle" class="txtschd_s11" style="padding-top:5px; padding-bottom:5px;">
                <table width="100%" border="0" cellspacing="4" cellpadding="0">
                  <tr align="center" valign="middle">
                    <td align="left">
                      <xsl:element name="a">
                        <xsl:attribute name="href">
                          <xsl:value-of select="//PrevDayLink" />
                        </xsl:attribute>
                        <xsl:attribute name="title">
                          Go to Previous Day
                        </xsl:attribute>
                        <img src="../../images/calendar-previous.png"></img>
                      </xsl:element>
                    </td>
                    <td style="font-size:14px; font-weight:bold; font-family:arial;">
                      <xsl:value-of select="//BlastDate" />
                    </td>
                    <td align="right">
                      <xsl:element name="a">
                        <xsl:attribute name="href">
                          <xsl:value-of select="//NextDayLink" />
                        </xsl:attribute>
                        <xsl:attribute name="title">
                          Go to Next Day
                        </xsl:attribute>
                        <img src="../../images/calendar-next.png"></img>
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
            <table width="100%" cellspacing="0" cellpadding="5" style="border-color: #CDCDCD; border-style: solid; border-width: thin 1px;">
            <xsl:for-each select="//DayBlast">
              <tr valign="top">
                   <td width="60" align="left" valign="middle" class="blugrid_col1" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;border-right-color: #CDCDCD; border-right-style: solid; border-width: thin 1px;">                  
                  <b style="font-size:12px; font-family:arial;"><xsl:value-of select="Header" /></b>                 
                </td>
                <td align="left" valign="middle" class="blugrid_col2 blugrid_div" style="border-bottom-color: #CDCDCD; border-bottom-style: solid; border-width: thin 1px;">
                  &#160; 
                  <xsl:for-each select="Subject">
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
              </tr>
            </xsl:for-each>
          </table>
        </td>
      </tr>
      <tr>
        <td align="left" valign="top" class="sccorner_shd1_botline">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr align="left" valign="top">
              <td>
                &#160;
              </td>
              <td align="right">
                &#160;
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>