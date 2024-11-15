<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Form_M_USER.aspx.vb" Inherits="KeihiWeb.Form_M_USER" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
     /* --- マウスオーバー時 --- */
table#bumon tr:hover {
background-color: #CC99FF; /* 行の背景色 */
}
</style>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/common.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_user.js" ></script>
<script type="text/javascript" src="../Scripts/WebForms/STSCommon/keihi_bumon.js" ></script>

<script type="text/javascript">

    $(function () {

        $('#d_user_id').css('ime-mode', 'inactive');
        $('#d_user_name').css('ime-mode', 'active');
        $('#d_user_kana').css('ime-mode', 'active');
        $('#d_password').css('ime-mode', 'inactive');
        $('#d_user_level').css('ime-mode', 'inactive');
        $('#d_bumon_cd').css('ime-mode', 'inactive');
        $('#d_del_flg').css('ime-mode', 'inactive');



        $('#d_bumon_cd').click(function () {
            $(this).select();
        });

        $('#d_del_flg').click(function () {
            $(this).select();
        });
        $('#d_user_level').click(function () {
            $(this).select();
        });
        $('#d_user_id').click(function () {
            $(this).select();
        });
        $('#d_user_name').click(function () {
            $(this).select();
        });
        $('#d_user_kana').click(function () {
            $(this).select();
        });
        $('#d_password').click(function () {
            $(this).select();
        });



        //部門検索画面 
        var di_bumon =$('#dialog_bumon');
        di_bumon.dialog({
            modal: true,
            width: 500,
            height: 400,
            autoOpen: false,
            buttons:{ 
                '閉じる': function () {
                    $(this).dialog('close');
                }
            }
        });

        //登録画面
        var di = $('#dialog_input');
        //ダイアログを初期化（自動オープンしない）
        di.dialog({
            modal: true,
            width: 500,
            height: 430,
            autoOpen: false,
            buttons: {

                '更新': function () {
                    var userid = document.getElementById("d_user_id").value;
                    var username = document.getElementById("d_user_name").value;
                    var userkana = document.getElementById("d_user_kana").value;
                    var userlvl = document.getElementById("d_user_level").value;
                    var password = document.getElementById("d_password").value;
                    var bumon_cd = document.getElementById("d_bumon_cd").value;
                    var del_flg = document.getElementById("d_del_flg").value;
                    var gyo = document.getElementById("d_gyo").value;
                    var kubun = document.getElementById("d_kubun").innerHTML;
                    var msg_red = document.getElementById("msg_red").innerHTML;

                    var JSONdata = {
                        user_id: userid,
                        user_name:username ,
                        user_kana:userkana,
                        user_level: userlvl,
                        password: password,
                        bumon_cd: bumon_cd,
                        del_flg: del_flg,
                        msg_red: msg_red
                    };

                    if (kubun === "新規モード") {
                        url_wk = 'Form_M_USER.aspx/InsertJOSNData';
                    }
                    if (kubun === "編集モード") {
                        url_wk = 'Form_M_USER.aspx/UpdateJOSNData';
                    }
                  
                    $.ajax({
                        type: "POST",
                        url: url_wk,
                        data: JSON.stringify(JSONdata),
                        datatype: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (msg) {

                            var msg_status = document.getElementById("msg_status");
                            var member = msg.d;
                            var result = member.result;
                           // var msglabel = document.getElementById("msglabel");
                            var wk_msg = document.getElementById("msg");
                            //グリッドに更新値をセット
                            if (kubun === "編集モード") {
                                var tr = $("#MainContent_GridView1 tr");
                                var cells = tr.eq(gyo).children();
                                cells.eq(1).text(userid);
                                cells.eq(2).text(username);
                                cells.eq(3).text(userkana);
                                cells.eq(4).text(password);
                                cells.eq(5).text(userlvl);
                                cells.eq(6).text(bumon_cd);
                                cells.eq(7).text(del_flg);
                            }
                            //結果をメッセージボッツクスへ
                            wk_msg.innerHTML = member[0].msg;
                            msg_status.value = member[0].result;
                            console.log(member[0].result);
                            if (msg_status.value === 'NG') {
                                di_msg.dialog('open');
                            }
                            return msg;
                        },
                        error: function (data, errorThrown) {
                            $('#msg').text('異常終了！！' + errorThrown);
                            di_msg.dialog('open');

                        }
                    }).done(function (data) {
                            var msg_status = document.getElementById("msg_status");
                            if (msg_status.value === 'OK') {
                                window.location.reload();
                            }
                    });

                },
                '閉じる': function () {
                    $(this).dialog('close');
                }
            }
        });

        //メッセージ用ダイアログ
        var di_msg = $('#dialog_msg');
        //ダイアログを初期化（自動オープンしない）
        di_msg.dialog({

            modal: false,
            width: 250,
            height: 170,
            autoOpen: false,
            buttons: {
               
                '閉じる': function () {
                    var msg_status = document.getElementById("msg_status");

                    //window.location.reload();
                    $(this).dialog('close');
                    if (msg_status.value === 'OK') {
                        di.dialog('close');
                    }
                }
            }
        });
        
        //削除確認ダイアログ
        var di_delete = $('#dialog_delete');
        //ダイアログを初期化（自動オープンしない）
        di_delete.dialog({
            modal: false,
            width: 250,
            height: 200,
            autoOpen: false,
            buttons: {
                '削除': function () {

                        var userid = document.getElementById("d_user_id").value;
                        var JSONdata = {
           　             user_id: userid,
             　　       };

                 　　   url_wk = 'Form_M_USER.aspx/DeleteJOSNData';
                    　　$.ajax({
                        　 type: "POST",
                        　 url: url_wk,
                           data: JSON.stringify(JSONdata),
                           datatype: 'json',
                           contentType: 'application/json; charset=utf-8',
                           success: function (msg) {

                            var member = msg.d;
                            var result = member.result;
                           // var msglabel = document.getElementById("msglabel");
                            var wk_msg = document.getElementById("msg");
                            //グリッドに更新値をセット
                            //結果をメッセージボッツクスへ
                            wk_msg.innerHTML = member[0].msg;
                            if (msg_status.value === 'NG') {
                                di_msg.dialog('open');
                            }
                            return msg;
                          },
                           error: function (result, errorThrown) {

                               $('#msg').text('異常終了！！' + errorThrown);
                               di_msg.dialog('open');

                           }

                    }).done(function (data) {

                        window.location.reload();
                    
                    });

                },
                '閉じる': function () {
                    //window.location.reload();
                    $(this).dialog('close');

                }
            }
        });

  //*********下段３つのボタンの処理
        // 新規ボタンのイベントハンドラ
    $('#<%=newuser.ClientID%>').click(function () {

            $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
            $('#d_user_id').prop('disabled', false);
            
            var wk_USER_ID = document.getElementById("d_user_id");
            var wk_USER_NAME = document.getElementById("d_user_name");
            var wk_USER_KANA = document.getElementById("d_user_kana");
            var wk_PASSWORD = document.getElementById("d_password");
            var wk_USER_LEVEL = document.getElementById("d_user_level");
            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_DEL_FLG = document.getElementById("d_del_flg");
            var wk_KUBUN = document.getElementById("d_kubun");
            wk_USER_ID.value = '';
            wk_USER_NAME.value = '';
            wk_USER_KANA.value = '';
            wk_PASSWORD.value = '';
            wk_USER_LEVEL.value = '';
            wk_BUMON_CD.value = '';
            wk_DEL_FLG.value = '0'
            wk_KUBUN.innerHTML = '';
            wk_BUMON_NAME.innerHTML = '';

            di.dialog('open');

            var wk_kubun = document.getElementById("d_kubun");
            wk_kubun.innerHTML = "新規モード";
        
            return false;
        });

    //編集ボタンクリック
    $('#<%=edituser.ClientID%>').click(function (e) {
        var chk = '0';
        $('#<%=GridView1.ClientID%> input:radio').each(function () {
           if ($(this).prop('checked') === true) {

                chk = '1';
                  
           }
        });

        if (chk === '1') {
            var wk_GYO = document.getElementById("d_gyo");
            var gyo = wk_GYO.value;
            var tr = $('#<%=GridView1.ClientID%> tr');
            var cells = tr.eq(gyo).children();
                  
            var user_id = cells.eq(1).text();//
            var user_name = cells.eq(2).text();//
            var user_kana = cells.eq(3).text();//
            var user_password = cells.eq(4).text();//
            var user_level = cells.eq(5).text();//
            var bumon_cd = cells.eq(6).text();//
            var del_flg = cells.eq(7).text();//

            //空白変換
            user_name = nbsp_func(user_name);
            user_kana = nbsp_func(user_kana);
            user_password = nbsp_func(user_password);

            //データをクリア
        
            di.dialog('open');

            var wk_USER_ID = document.getElementById("d_user_id");
            var wk_USER_NAME = document.getElementById("d_user_name");
            var wk_USER_KANA = document.getElementById("d_user_kana");
            var wk_PASSWORD = document.getElementById("d_password");
            var wk_USER_LEVEL = document.getElementById("d_user_level");
            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_DEL_FLG = document.getElementById("d_del_flg");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            //部門名表示用関数
            //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            $('d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);

            wk_USER_ID.value = user_id;
            wk_USER_NAME.value = user_name;
            wk_USER_KANA.value = user_kana;
            wk_PASSWORD.value = user_password;
            wk_USER_LEVEL.value = user_level;
            wk_BUMON_CD.value = bumon_cd;
            wk_DEL_FLG.value = del_flg;
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = gyo;

            $('#d_user_id').prop('disabled', true);
            $('#d_user_name').focus;


        } else {

            $('#msg').text('行選択されていません');
            di_msg.dialog('open');
            return false;

        }
        return false;

    });


        //行クリックでラジオON
        $("#MainContent_GridView1 td").click(function () {
            //縦
            var row = $(this).closest('tr').index();
            //横
            var col = this.cellIndex;
  
            $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
            $(this).closest("tr").find("td input:radio").each(function () {
               $(this).prop('checked', true);
            });

            var user_id = $(this).closest("tr").find("td").eq(1).text();
            var user_name = $(this).closest("tr").find("td").eq(2).text();
            var user_kana = $(this).closest("tr").find("td").eq(3).text();
            var user_password = $(this).closest("tr").find("td").eq(4).text();
            var user_level = $(this).closest("tr").find("td").eq(5).text();
            var bumon_cd = $(this).closest("tr").find("td").eq(6).text();
            var del_flg = $(this).closest("tr").find("td").eq(7).text();

            var wk_USER_ID = document.getElementById("d_user_id");
            var wk_USER_NAME = document.getElementById("d_user_name");
            var wk_USER_KANA = document.getElementById("d_user_kana");
            var wk_PASSWORD = document.getElementById("d_password");
            var wk_USER_LEVEL = document.getElementById("d_user_level");
            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_DEL_FLG = document.getElementById("d_del_flg");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            //部門名表示用関数
            //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            $('#d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);

            wk_USER_ID.value = user_id;
            wk_USER_NAME.value = user_name;
            wk_USER_KANA.value = user_kana;
            wk_PASSWORD.value = user_password;
            wk_USER_LEVEL.value = user_level;
            wk_BUMON_CD.value = bumon_cd;
            wk_DEL_FLG.value = del_flg;
            wk_KUBUN.innerHTML = "編集モード";
            wk_GYO.value = row;

            $('#d_user_id').prop('disabled', true);
            $('#d_user_name').focus;

       
        });


        //ユーザーマスタ
        //行ダブルクリックで編集画面
        $("#MainContent_GridView1 td").dblclick(function () {
            //縦
            var row = $(this).closest('tr').index();
            //横
            var col = this.cellIndex;
            var user_id= $(this).closest("tr").find("td").eq(1).text();
            var user_name = $(this).closest("tr").find("td").eq(2).text();
            var user_kana = $(this).closest("tr").find("td").eq(3).text();
            var user_password = $(this).closest("tr").find("td").eq(4).text();
            var user_level = $(this).closest("tr").find("td").eq(5).text();
            var bumon_cd = $(this).closest("tr").find("td").eq(6).text();
            var del_flg = $(this).closest("tr").find("td").eq(7).text();

            //空白変換
            user_name = nbsp_func(user_name);
            user_kana = nbsp_func(user_kana);
            user_password = nbsp_func(user_password);

            //編集画面オープン
            di.dialog('open');
      
            var wk_USER_ID = document.getElementById("d_user_id");
            var wk_USER_NAME = document.getElementById("d_user_name");
            var wk_USER_KANA = document.getElementById("d_user_kana");
            var wk_PASSWORD = document.getElementById("d_password");
            var wk_USER_LEVEL = document.getElementById("d_user_level");
            var wk_BUMON_CD = document.getElementById("d_bumon_cd");
            var wk_BUMON_NAME = document.getElementById("d_bumon_name");
            var wk_DEL_FLG = document.getElementById("d_del_flg");
            var wk_KUBUN = document.getElementById("d_kubun");
            var wk_GYO = document.getElementById("d_gyo");

            //部門名表示用関数
            //$('GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
            $('d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);

            wk_USER_ID.value = user_id;
            wk_USER_NAME.value = user_name;
            wk_USER_KANA.value = user_kana;
            wk_PASSWORD.value = user_password;
            wk_USER_LEVEL.value = user_level;
            wk_BUMON_CD.value = bumon_cd;
            wk_DEL_FLG.value = del_flg;
            wk_KUBUN.innerHTML  = "編集モード";
            wk_GYO.value = row;

            $('#d_user_id').prop('disabled', true);
            $('#d_user_name').focus;

            
        });

        $('#bumon').click(function () {
            di_bumon.dialog('close');
        });

        //部門ｺｰﾄﾞのチェンジイベント
        //部門名称表示
        $('#d_bumon_cd').on({
            change: function () {
                var msg_red = document.getElementById("msg_red");
                msg_red.innerHTML = '';

                var bumon_cd = document.getElementById("d_bumon_cd").value;

                var wk_BUMON_NAME = document.getElementById("d_bumon_name");
                if (bumon_cd.length > 0) {
                    //部門名表示用関数
                    //$('#GridView1').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
                    $('#d_bumon_name').func_bumon_nm(bumon_cd, wk_BUMON_NAME);
                } else {
                    $('#d_bumon_name').text('');
                }
               
            }
        });
        //部門選択画面オープン
        $('#d_btn_serch').click(function (e) {

            var msg_red = document.getElementById("msg_red");
            msg_red.innerHTML = '';
            $('table#bumon *').remove();

            $.ajax({
                type: "POST",
                url: "Form_M_USER.aspx/GetJOSNData_Bumon",
                contentType: "application/json;charset=utf-8",
                data: {},
                dataType: "json",
                success: function (data) {

                    if (data.d.length > 0) {
                        $('#bumon').append("<tr style='background-color:#6699FF'><th>部門コード</th><th>部門名</th><th>債務部門</th></tr>");

                      for (var i = 0; i < data.d.length; i++) {
                             $('#bumon').append("<tr onClick='mClickTR(this)'><td>" + data.d[i].BUMON_CD + "</td> <td>" + data.d[i].BUMON_NM + "</td> <td>" + data.d[i].SAIMU_BMN + "</td></tr>");
                         
                      }
                    }
                },
                error: function (result, errorThrown) {

                    $('#msg').text('異常終了！！' + errorThrown);
                    di_msg.dialog('open');

                }
         
            });
            di_bumon.dialog('open');

        });
    
    
    // 行削除ボタンのイベントハンドラ
    $('#<%=deluser.ClientID%>').click(function () {
        //wk_kubun.innerHTML = "削除モード";
        var wk_GYO = document.getElementById("d_gyo").value;

        if (wk_GYO.length === 0) {
            var msg = document.getElementById("msg");
            msg.innerHTML = '行選択してください';

            di_msg.dialog('open');
            return false
        }
  
        di_delete.dialog('open');
        return false;
    });


    //
    //行クリック選択ラジオンボタンの解除とチェック
    //
    $("#MainContent_GridView1 td").click(function () {

         $('#<%=GridView1.ClientID%> input:radio').each(function () {
                $(this).prop('checked', false);
            });
         $(this).closest("tr").find("td input:radio").each(function () {
               $(this).prop('checked', true);
         });

        //選択行をキープ
         var wk_GYO = document.getElementById("d_gyo");
         var row = $(this).closest('tr').index();
         wk_GYO.value = row;
    
    });
  



    });

    //部門選択するイベント
    function mClickTR(obj) {
    
            var cd = document.getElementById('d_bumon_cd') ;
            var nm = document.getElementById('d_bumon_name');
            console.log(cd);
            console.log(nm);
            cd.value = obj.cells[0].innerHTML;
            nm.innerHTML = obj.cells[1].innerHTML;
            $(nm).css('color', 'black');
    }

</script>
    <div id="Container">
      <div class="index1">
    ユーザー登録
        </div>
<asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" HorizontalAlign="Center" Width="100%"
        Height="400px" >
    <asp:GridView ID="GridView1" runat="server" HorizontalAlign="Center" Width="100%">
         <Columns>
            <asp:TemplateField HeaderText="選択" HeaderStyle-HorizontalAlign="Center">
                <ItemTemplate >
                    <asp:RadioButton ID="RadioButton1" runat="server"  />
                    </ItemTemplate>
         </asp:TemplateField>
          </Columns>
        <HeaderStyle cssclass="fixdetail" Wrap="False" BackColor="#99CCFF" />
    </asp:GridView>
    </asp:Panel>
  
<div style="margin-top:5px">
<asp:button runat="server"  class="btn btn-default" id="newuser" text="新規" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="edituser"  text="編集" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="deluser" text="削除" style="height:50px"/>
<asp:button runat="server"  class="btn btn-default" id="closeuser" text="閉じる"  style="height:50px"/>
</div>
    
<div id="dialog_input" title="新規/編集">
<p>
<label id="d_kubun" style="color:red;">種別</label>
</p>
<div class="hi">
<input type="hidden" id="d_gyo" />
<table border ="0">
<tr>
<td>ユーザーＩＤ</td><td><input type="text"id="d_user_id" class="input-sm" style="font-size:small;width:100px;"/></td>
</tr>
<tr>
<td>ユーザー名</td><td><input type="text" id="d_user_name" class="input-sm" style="font-size:small;width:300px;" />
</tr>
<tr>
<td>カナ名称</td><td><input type="text" id="d_user_kana" class="input-sm" style="font-size:small;width:300px;"/>
</tr>
<tr> 
<td>パスワード</td><td><input type="text" id="d_password" class="input-sm" style="font-size:small;width:100px;"/>
</tr>
<tr>
<td>ユーザーレベル</td><td><input type="text" id="d_user_level" class="input-sm" style="font-size:small;width:100px;"/>
</tr>
<tr>
<td>部門コード</td><td><input type="text" id="d_bumon_cd" class="input-sm" style="font-size:small;width:100px;"/> <button id="d_btn_serch">検索</button></td>
</tr>
<tr>
<td>部門名</td>
<td>
<label id="d_bumon_name" style="height:10px ;font-size:smaller;color:blue" ></label>
</td>
</tr>
<tr>
<td >DELFLG 0:保存1:削除</td><td><input type="text" id="d_del_flg" class="input-sm" style="font-size:small;"/></td>
</tr>
</table>
<p></p>
<div id="msg_red" style="display:none;font-size:smaller ;color:red;"></div>
</div>
</div>
<div id="dialog_msg" title="メッセージ">
<label id="msg" style="font-size:small;"></label> 
<input id="msg_status" type="hidden" />
</div>
<div id="dialog_bumon" title="部門マスタ">
<table id="bumon" border="1">
</table>
</div>
<div id="dialog_delete" title="メッセージ">
削除します　よろしいですか?
</div>
<div>
</div>
    </div>    
</asp:Content>
