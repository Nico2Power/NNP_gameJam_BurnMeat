# 《BurnMeatYAKINIKU》
#**遊玩連結**
https://niconicopower.itch.io/burnmeatyakiniku

# **玩法發想**
以倖存者類型作為基礎構思，但不想以單純打怪與拉長生存時間作為玩家得分目標，  
而因時間關係則做成打怪得分類型的遊戲。  
未來想增加經營要素，例如需要將打倒的敵人製作成特定餐點，滿足等待的客人。

# **遊戲目標**
將肉烤熟並得分，盡可能延長生存時間與增加得分。  

# **故事大綱**
主角是一名打拼數年終於自己一家燒烤店的店主，
由於神祕力量使得放在冷藏室的肉開始會移動並襲擊主角。  
但已經將身家通通付諸於此店，已是背水一戰，
於是拿起噴火槍將襲擊的肉烤熟。

烤熟後，主角見肉並無異狀，便大膽地嘗了一口，
這些受到神秘力量控制過後的肉居然非常好吃，  
而經過店家的路人因香氣誘惑而進入店內想品嘗佳餚，
就此展開了一場與肉與生計的拚搏。

# **遊戲設計說明**  
## **場景說明**  
![image](https://github.com/user-attachments/assets/1a4a4831-2a14-479e-87d2-c8eb6395a318)  
  
1.玩家：玩家操控的主角。  
2.玩家狀態(彈藥與生命值)：玩家的彈藥與生命值。  
3.得分點：將熟肉搬運至此，來得到分數。  
4.垃圾桶：將焦肉搬運至此丟棄。  
5.計時器：紀錄玩家生存時間。  
6.得分：紀錄搬運成功至得分點熟肉之數量。  

##  **遊戲操控**
方向鍵:移動角色  
Z:搬運  
X:揮擊  
C:噴火  
V:換彈  
C+Z(Hold)可使噴火時緩速移動  

## **戰鬥設計**  
### **噴火**  
使用按鍵C操控GameObject的Active state，進行噴火槍的攻擊，
敵人受到攻擊會以固定頻率減少生命值。
在噴火狀態同時按按鍵Z，可使主角進行緩速移動，使走位更加方便。  
![image](https://github.com/user-attachments/assets/43315dcb-3a98-4b6e-98d2-3f0c90b376ad)  
  
  
### **揮擊**  
使用按鍵X操控GameObject的Active state，進行揮舞夾子的攻擊，  
敵人受到攻擊會被擊退，使玩家有逃跑外的求生選項。  
![image](https://github.com/user-attachments/assets/b306f034-3dbf-48a9-a2ce-9a7b6e29cc2f)  
  
  
### **搬運**  
使用按鍵Z將接觸到的烤熟肉或烤焦肉進行搬運。  
功能做法是以接觸到GameObject碰撞框時，會將記錄碰觸之GameObject，  
在此時使用按鍵Z就會搬起所接觸的GameObject。  
離開GameObject碰撞框時，則會記錄GameObject之變數設為null。  
![image](https://github.com/user-attachments/assets/5b00cb7c-0f73-48ef-8777-4183bb09cd07)  
  
  
## **敵人設計**  
![image](https://github.com/user-attachments/assets/23efd3d4-4662-4d87-86b8-e187abfbcb40)
敵人有三個變數，分別為冷凍度(初始為100)﹑熟度(初始為100)與腐敗度(初始為0)。  
被火焰攻擊則會先扣除冷凍度(-5)，冷凍度歸零時則開始扣除熟度(生肉-10/熟肉-5)，  
在非冷凍肉狀態時，如無受到攻擊或搬運腐敗度則上升(+5/sec)。  
  
變數改變到一定條件時，會使肉的狀態進行變化，以下進行說明：  
1.冷凍肉：會追蹤並攻擊玩家，在冷凍度歸零時會變成生肉。  
  
2.生肉：無法搬運、得分或丟棄，會隨著時間增加腐敗度。  
受到火焰攻擊會扣除熟度，到一定值則會變成熟肉(熟度<=30)。  
  
3.熟肉：可以搬運並進行得分，無法丟棄，會隨著時間增加腐敗度。  
受到火焰攻擊會扣除熟度，到一定值則會變成焦肉(熟度<=70)。  
  
4.焦肉：可以搬運並進行丟棄，無法得分，會隨著時間增加腐敗度。    
  
5.腐敗肉︰在非冷凍肉狀態下會持續增加腐敗度，  
當受到攻擊或搬運腐敗度則會歸零，而當腐敗度到100時則會成為腐敗肉。  
會追蹤並攻擊玩家，受到火焰攻擊會扣除腐敗度(-10)，歸零時則會變成焦肉。  
  
## **程式實作**
### **揮擊**  
![image](https://github.com/user-attachments/assets/e8011d19-15d9-4eba-a78a-bed83abbdb0f)  
![image](https://github.com/user-attachments/assets/53862c4e-40c5-4cb4-8cc9-5db831850289)  
  
### **搬運**
![image](https://github.com/user-attachments/assets/ffe53520-6845-4b11-904a-8217712c7676)  
![image](https://github.com/user-attachments/assets/92de39a9-fa71-4b9a-83e9-466e3640eb2d)  
![image](https://github.com/user-attachments/assets/7ad4772f-2fe8-4783-b9eb-22112a6d7d18)  





