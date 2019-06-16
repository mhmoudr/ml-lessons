import matplotlib.pyplot as plt
import pandas as pd
import numpy as np
import json
from sklearn.ensemble import RandomForestClassifier, RandomForestRegressor

#pathToDataFile = '~/data/clean_data.csv'

pathToDataFile = '~/data/heart.csv'

data = pd.read_csv(pathToDataFile)
msk = np.random.rand(len(data)) < 0.8
train = data[msk]
validate = data[~msk]

data.describe()

data.head()

data.as_matrix()[:,5] #reading column

h = np.histogram(data.as_matrix()[:,1])
plt.hist(h)
plt.show()

data_ex_na = data.dropna()
plt.rcParams["font.size"] = 24
data.hist(bins=50,figsize=(50,50))
plt.show()
len (data)

rf = RandomForestClassifier()
rf.fit(train.factorize(),validate)




d = [1,1,1,1,1,2,3,4,10000]
np.histogram(d,bins=10)




##############
import matplotlib.pyplot as plt
import xgboost as xgb


X = data.as_matrix()[:,0:13]
Y = data.as_matrix()[:,13]

dtrain = xgb.DMatrix( '/Users/mahmoudr/data/heart.csv?format=csv&label_column=13')
##dtest  = xgb.DMatrix('/tmp/data/HBA_holdout.csv?format=csv&label_column=0')
#evallist = [(dtest, 'eval'), (dtrain, 'train')]
evallist = [(dtrain, 'train')]


param = {'max_depth': 10, 'eta': 0.03, 'nthread':10,'objective': 'binary:logistic'}
num_round = 100
model = xgb.train(param, dtrain, num_round, evallist)
model.dump_model('/Users/mahmoudr/playground/ml-lessons/l04-decision-tree-improved/model.txt')
model.get_dump(dump_format="json")

xgb.plot_importance(model)
xgb.plot_tree(model, num_trees=100)
xgb.to_graphviz(model, num_trees=100)

